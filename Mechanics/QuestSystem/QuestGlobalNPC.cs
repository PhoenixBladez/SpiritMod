using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Ammo.Arrow;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.NPCs.Town;
using SpiritMod.Items.Consumable.Quest;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestGlobalNPC : GlobalNPC
	{
		public static event Action<NPC> OnNPCLoot;
		public static event Action<int, Chest, int> OnSetupShop;

		public static Dictionary<int, QuestPoolData> SpawnPoolMods = new Dictionary<int, QuestPoolData>();
		public static Dictionary<int, int> PoolModsCount = new Dictionary<int, int>();

		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.Zombie || npc.type == NPCID.BaldZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.TwiggyZombie || npc.type == NPCID.ZombieRaincoat || npc.type == NPCID.PincushionZombie || npc.type == NPCID.ZombieEskimo)
			{
				if (!QuestManager.GetQuest<ZombieOriginQuest>().IsUnlocked && QuestManager.GetQuest<FirstAdventure>().IsCompleted && Main.rand.Next(40) == 0)
				{
					int slot = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<OccultistMap>());

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, slot, 1f);
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
				ClientNPCLoot(npc);
			else if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
				packet.Write((byte)QuestMessageType.SyncOnNPCLoot);
				packet.Write(false);
				packet.Write((byte)npc.whoAmI);
				packet.Send(-1, 256);
			}
		}

		/// <summary>Contains everything needed for clients to know what NPC died without needing untoward syncing. If you need to add quest queues, do it here.</summary>
		public void ClientNPCLoot(NPC npc)
		{
			if (npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.SkeletronHead || npc.type == ModContent.NPCType<NPCs.Boss.Scarabeus.Scarabeus>() ||
				npc.type == ModContent.NPCType<NPCs.Boss.AncientFlyer>() || npc.type == ModContent.NPCType<NPCs.Boss.MoonWizard.MoonWizard>() || npc.type == ModContent.NPCType<NPCs.Boss.SteamRaider.SteamRaiderHead>())
			{
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<SlayerQuestOccultist>());
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<UnidentifiedFloatingObjects>());
			}

			if (npc.type == NPCID.EaterofWorldsHead)
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<SlayerQuestMarble>());

			if (npc.type == NPCID.SkeletronHead)
			{
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<RaidingTheStars>());
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<Adventurer>(), QuestManager.GetQuest<StrangeSeas>());
				ModContent.GetInstance<QuestWorld>().AddQuestQueue(ModContent.NPCType<RuneWizard>(), QuestManager.GetQuest<IceDeityQuest>());
			}

			OnNPCLoot?.Invoke(npc);
		}

		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			if (type == ModContent.NPCType<RuneWizard>() && QuestManager.GetQuest<FirstAdventure>().IsCompleted && !Main.dayTime)
				shop.item[nextSlot++].SetDefaults(ModContent.ItemType<OccultistMap>(), false);

			if (type == NPCID.Stylist)
			{
				if (QuestManager.GetQuest<StylistQuestSeafoam>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.SeafoamDye>(), false);

				if (QuestManager.GetQuest<StylistQuestMeteor>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.MeteorDye>(), false);
				if (QuestManager.GetQuest<StylistQuestCorrupt>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.CystalDye>(), false);
				if (QuestManager.GetQuest<StylistQuestCrimson>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.DyesMisc.HairDye.ViciousDye>(), false);
			}
			if (type == NPCID.Merchant)
			{
				if (QuestManager.GetQuest<AnglerStatueQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<GiantAnglerStatue>(), false);
			}
			if (type == NPCID.Demolitionist)
			{
				if (QuestManager.GetQuest<RescueQuestStylist>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LongFuse>(), false);
			}
			if (type == ModContent.NPCType<Adventurer>())
			{
				if (QuestManager.GetQuest<FirstAdventure>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<DurasilkSheaf>(), false);

				if (QuestManager.GetQuest<ExplorerQuestAsteroid>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ExplorerScrollAsteroidFull>(), false);
				if (QuestManager.GetQuest<ExplorerQuestGranite>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ExplorerScrollGraniteFull>(), false);
				if (QuestManager.GetQuest<ExplorerQuestMarble>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ExplorerScrollMarbleFull>(), false);
				if (QuestManager.GetQuest<ExplorerQuestHive>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ExplorerScrollHiveFull>(), false);
				if (QuestManager.GetQuest<ExplorerQuestMushroom>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ExplorerScrollMushroomFull>(), false);

				if (QuestManager.GetQuest<CritterCaptureFloater>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Weapon.Magic.LuminanceSeacone.LuminanceSeacone>(), false);

				if (QuestManager.GetQuest<ManicMage>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Sets.MagicMisc.Lightspire.AkaviriStaff>(), false);

				if (QuestManager.GetQuest<SkyHigh>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Tiles.Furniture.JadeDragonStatue.DragonStatueItem>(), false);

				if (QuestManager.GetQuest<SlayerQuestCavern>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Weapon.Thrown.ClatterSpear>(), false);

				if (QuestManager.GetQuest<ZombieOriginQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Weapon.Swung.Punching_Bag.Punching_Bag>(), false);

				if (QuestManager.GetQuest<DecrepitDepths>().IsCompleted)
				{
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SepulchreArrow>(), false);
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SepulchreBannerItem>(), false);
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SepulchreChest>(), false);
				}

				if (QuestManager.GetQuest<SkyHigh>().IsCompleted)
				{
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<PottedSakura>(), false);
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<PottedWillow>(), false);
				}
				if (QuestManager.GetQuest<ItsNoSalmon>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<KoiTotem>(), false);
				if (QuestManager.GetQuest<SporeSalvage>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Tiles.Furniture.Critters.VibeshroomJarItem>(), false);
				if (QuestManager.GetQuest<SlayerQuestDrBones>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.SeedBag>(), false);
				if (QuestManager.GetQuest<IceDeityQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Weapon.Thrown.CryoKnife>(), false);
				if (QuestManager.GetQuest<IceDeityQuest>().IsCompleted)
					shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Placeable.IceSculpture.IceDeitySculpture>(), false);
			}
			OnSetupShop?.Invoke(type, shop, nextSlot);
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) //Draws the exclamation mark on the NPC when they have a quest
		{
			bool valid = ModContent.GetInstance<SpiritClientConfig>().ShowNPCQuestNotice && npc.CanTalk; //Check if the NPC talks and if the config allows
			if (valid && ModContent.GetInstance<QuestWorld>().NPCQuestQueue.ContainsKey(npc.type) && ModContent.GetInstance<QuestWorld>().NPCQuestQueue[npc.type].Count > 0)
			{
				Texture2D tex = Mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
				float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
				spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);
			}
		}

		public static void AddToPool(int id, QuestPoolData data)
		{
			if (SpawnPoolMods.ContainsKey(id))
			{
				QuestPoolData currentData = SpawnPoolMods[id];
				if (SpawnPoolMods[id].NewRate is null || (data.NewRate != null && SpawnPoolMods[id].NewRate < data.NewRate))
					currentData.NewRate = data.NewRate;

				PoolModsCount[id]++;
			}
			else
			{
				SpawnPoolMods.Add(id, data);
				PoolModsCount.Add(id, 1);
			}
		}

		public static void RemoveFromPool(int id)
		{
			if (SpawnPoolMods.ContainsKey(id))
			{
				PoolModsCount[id]--;

				if (PoolModsCount[id] == 0)
				{
					SpawnPoolMods.Remove(id);
					PoolModsCount.Remove(id);
				}
			}
		}

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			foreach (int item in SpawnPoolMods.Keys)
			{
				var currentPool = SpawnPoolMods[item];
				if (!pool.ContainsKey(item))
				{
					if (currentPool.Forced)
					{
						if (((currentPool.Exclusive && !NPC.AnyNPCs(item)) || !currentPool.Exclusive) && (currentPool.Conditions == null || currentPool.Conditions.Invoke(spawnInfo)))
							pool.Add(item, currentPool.NewRate.Value);
					}
					return;
				}

				if (currentPool.NewRate is null) //We don't have a new rate to set to
					return;

				if (((currentPool.Exclusive && !NPC.AnyNPCs(item)) || !currentPool.Exclusive) && (currentPool.Conditions == null || currentPool.Conditions.Invoke(spawnInfo)))
					pool[item] = currentPool.NewRate.Value;
			}

			if (QuestManager.GetQuest<SlayerQuestClown>().IsActive)
			{
				if (pool.ContainsKey(NPCID.Clown))
					pool[NPCID.Clown] = 0.1f;
			}

			if (QuestManager.GetQuest<SlayerQuestDrBones>().IsActive)
			{
				if (!Main.dayTime && spawnInfo.Player.ZoneJungle && !spawnInfo.PlayerSafe && spawnInfo.SpawnTileY < Main.worldSurface && !NPC.AnyNPCs(NPCID.DoctorBones) && pool.ContainsKey(NPCID.DoctorBones))
					pool[NPCID.DoctorBones] = 0.1f;
			}

			if (QuestManager.GetQuest<SlayerQuestDrBones>().IsActive)
			{
				if (spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.PlayerSafe && spawnInfo.SpawnTileY > Main.rockLayer && !NPC.AnyNPCs(NPCID.LostGirl) && pool.ContainsKey(NPCID.LostGirl))
					pool[NPCID.LostGirl] = 0.05f;
			}
		}
	}
}