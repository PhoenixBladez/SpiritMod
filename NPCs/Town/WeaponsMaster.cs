using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Weapon.Swung.AnimeSword;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SpiritMod.NPCUtils;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	[AutoloadHead]
	public class WeaponsMaster : ModNPC
	{
		public override string Texture => "SpiritMod/NPCs/Town/WeaponsMaster";

		public override string[] AltTextures => new string[] { "SpiritMod/NPCs/Town/WeaponsMaster_Alt_1" };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Legendary Weapons Master");
			Main.npcFrameCount[npc.type] = 26;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 1500;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 16;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.Guide);
			npc.townNPC = false;
			npc.friendly = true;
			npc.aiStyle = 7;
			npc.damage = 30;
			npc.defense = 30;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			animationType = NPCID.Guide;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
                QuestWorld.downedWeaponsMaster = true;
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Bandit/Bandit3"));
			}
		}
        int npcTimer;
        bool checkKill;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npcTimer);
			writer.Write(checkKill);
        }
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npcTimer = reader.ReadInt32();
			checkKill = reader.ReadBoolean();
        }
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            float distance = Vector2.Distance(npc.Center, player.Center);
            if (distance <= 160f)
            {
                checkKill = true;
            }
            if (checkKill)
            {
                npcTimer++;
                if (npcTimer % 20 == 0)
                {
                    npc.StrikeNPC(100, 0f, 0);
                    SpiritMod.primitives.CreateTrail(new AnimePrimTrailTwo(npc)); 
                }
            }
        }
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Quest.WeaponMasterHead>());
        }
		/*public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.player.Any(x => x.active) && !NPC.AnyNPCs(NPCType<Rogue>()) && !NPC.AnyNPCs(NPCType<BoundRogue>());
		}*/
		public override string TownNPCName()
		{
			string[] names = { "Magnus Mustafa Thrax III" };
			return Main.rand.Next(names);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<WeaponsMaster>()) || !QuestManager.GetQuest<MurderMysteryQuest>().IsActive || QuestWorld.downedWeaponsMaster)
			{
				return 0f;
			}
			return SpawnCondition.OverworldNightMonster.Chance * 5f;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) //Draws the exclamation mark on the NPC when they have a quest
		{
            Texture2D tex = mod.GetTexture("UI/QuestUI/Textures/ExclamationMark");
			float scale = (float)Math.Sin(Main.time * 0.08f) * 0.14f;
			spriteBatch.Draw(tex, new Vector2(npc.Center.X - 2, npc.Center.Y - 40) - Main.screenPosition, new Rectangle(0, 0, 6, 24), Color.White, 0f, new Vector2(3, 12), 1f + scale, SpriteEffects.None, 0f);

		}
	}
}
