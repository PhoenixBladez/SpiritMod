using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ModLoader;

using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo.Arrow;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pins;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.NPCs.Town;

using static Terraria.ModLoader.ModContent;

using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.QuestSystem
{
    public class QuestGlobalNPC : GlobalNPC
    {
		public static event Action<IDictionary<int, float>, NPCSpawnInfo> OnEditSpawnPool;
        public static event Action<NPC> OnNPCLoot;
        public static event Action<int, Chest, int> OnSetupShop;

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) => OnEditSpawnPool?.Invoke(pool, spawnInfo);

		public override void NPCLoot(NPC npc)
        {     
			if (npc.type == NPCID.Zombie || npc.type == NPCID.BaldZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.TwiggyZombie || npc.type == NPCID.ZombieRaincoat || npc.type == NPCID.PincushionZombie || npc.type == NPCID.ZombieEskimo) {
				if (!QuestWorld.zombieQuestStart && QuestManager.GetQuest<FirstAdventure>().IsCompleted)
					if (Main.rand.Next(40) == 0)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<OccultistMap>());
			}

            if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.SkeletronHead || npc.type == NPCType<NPCs.Boss.Scarabeus.Scarabeus>() || 
				npc.type == NPCType<NPCs.Boss.AncientFlyer>() || npc.type == NPCType<NPCs.Boss.MoonWizard.MoonWizard>() || npc.type == NPCType<NPCs.Boss.SteamRaider.SteamRaiderHead>())
            {
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<Adventurer>(), QuestManager.GetQuest<SlayerQuestOccultist>());
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<Adventurer>(), QuestManager.GetQuest<UnidentifiedFloatingObjects>());
            }

            if (npc.type == NPCID.EaterofWorldsHead)
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<Adventurer>(), QuestManager.GetQuest<SlayerQuestMarble>());

            if (npc.type == NPCID.SkeletronHead)
            {
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<Adventurer>(), QuestManager.GetQuest<RaidingTheStars>());
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<Adventurer>(), QuestManager.GetQuest<StrangeSeas>());
				GetInstance<QuestWorld>().AddQuestQueue(NPCType<RuneWizard>(), QuestManager.GetQuest<IceDeityQuest>());
			}
			OnNPCLoot?.Invoke(npc);
        }

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == NPCType<RuneWizard>() && QuestManager.GetQuest<FirstAdventure>().IsCompleted && !Main.dayTime)
				shop.item[nextSlot++].SetDefaults(ItemType<OccultistMap>(), false);

			if (type == NPCID.Stylist)
            {
				if (QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Items.Sets.DyesMisc.HairDye.SeafoamDye>(), false);

				if (QuestManager.GetQuest<StylistQuestMeteor>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Items.Sets.DyesMisc.HairDye.MeteorDye>(), false);
			}
			if (type == NPCType<Adventurer>())
			{
				if (QuestManager.GetQuest<DecrepitDepths>().IsCompleted) {
					shop.item[nextSlot++].SetDefaults(ItemType<SepulchreArrow>(), false);
					shop.item[nextSlot++].SetDefaults(ItemType<SepulchreBannerItem>(), false);
					shop.item[nextSlot++].SetDefaults(ItemType<SepulchreChest>(), false);
				}
				if (QuestManager.GetQuest<SkyHigh>().IsCompleted) {
					shop.item[nextSlot++].SetDefaults(ItemType<PottedSakura>(), false);
					shop.item[nextSlot++].SetDefaults(ItemType<PottedWillow>(), false);
				}

				if (QuestManager.GetQuest<SporeSalvage>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Tiles.Furniture.Critters.VibeshroomJarItem>(), false);
				if (QuestManager.GetQuest<SlayerQuestDrBones>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Items.Consumable.SeedBag>(), false);
				if (QuestManager.GetQuest<IceDeityQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Items.Weapon.Thrown.CryoKnife>(), false);
				if (QuestManager.GetQuest<IceDeityQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.IceSculpture.IceDeitySculpture>(), false);
			}
			OnSetupShop?.Invoke(type, shop, nextSlot);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor) //Draws the exclamation mark on the NPC when they have a quest
		{
			bool valid = GetInstance<SpiritClientConfig>().ShowNPCQuestNotice && npc.CanTalk; //Check if the NPC talks and if the config allows
			if (valid && GetInstance<QuestWorld>().NPCQuestQueue.ContainsKey(npc.type) && GetInstance<QuestWorld>().NPCQuestQueue[npc.type].Count > 0)
			{
				Texture2D tex = mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
				float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
			}
		}
	}
}
