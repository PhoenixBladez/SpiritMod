using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.FrozenSouls
{
	public class WintrySoul : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icebound Soul");
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 28;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60f;
			NPC.immortal = true;
			NPC.catchItem = (short)ModContent.ItemType<SoulOrbItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
			NPC.noGravity = true;
			AIType = NPCID.Firefly;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
				new FlavorTextBestiaryInfoElement("A curious memory of time past, somehow preserved by the deep icy tundra."),
			});
		}

		float decisionValue = 0f;
		float outcomeValue = 0f;

		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool CanChat() => true;
		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;

		public override string GetChat()
		{
			return "As I approach the spirit, the cave around me seems to shake. The walls collapse, leaving me standing in a boundless chasm. I am alone. Countless piles of gold and relics lay strewn about, covered in a thin layer of frost. The only light in the cavern seems to come from the soul beside me as it waits for me to make a decision.";
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			if (decisionValue == 0f)
			{
				button = "Take";
				button2 = "Leave";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (decisionValue == 0f)
					decisionValue = 1f;
			}
			else
			{
				if (decisionValue == 0f)
					decisionValue = 2f;
			}
		}

		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			float num366 = num395 + .85f;
			NPC.scale = num366;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .55f, .55f, .9f);
			if (decisionValue == 0f)
            {
				DoPassiveDust();
            }
			foreach (var player in Main.player)
			{
				if (!player.active) continue;
				if (player.talkNPC == NPC.whoAmI)
				{
					NPC.velocity = Vector2.Zero;
					if (decisionValue == 2f)
					{
						Main.npcChatText = "My vision returns to normal. The soul flits around me, apparently pleased with my decision. It gives me an ancient relic and other resources for my journey.";

						if (outcomeValue == 0f)
						{
							DoExplosionDust();
							SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/PositiveOutcome"), NPC.Center);
							outcomeValue = 1f;
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 13);
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 13);
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 13);
							if (!player.HasItem(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard1>()))
							{ 
								player.QuickSpawnItem(player.GetSource_GiftOrReward("Quest"), ItemID.WarmthPotion, 2);
								player.QuickSpawnItem(player.GetSource_GiftOrReward("Quest"), ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.IceDeityShard1>(), 1);
							}
						}
					}
					if (decisionValue == 1f)
					{
						Main.npcChatText = "My vision blurs. I start to shiver. I feel weak. The warmth that the soul gave me disappears slowly, and the soul melts away into the icy caverns. I should try looking elsewhere for the artifact. Perhaps I can scavenge the artifact from some Winterborn.";
						if (outcomeValue == 0f)
						{
							DoExplosionDust();
							SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/NegativeOutcome"), NPC.Center);
							outcomeValue = 1f;
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 99);
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 99);
							Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 99);
							player.AddBuff(BuffID.Darkness, 3600);
							player.AddBuff(BuffID.Weak, 3600);
							player.AddBuff(BuffID.BrokenArmor, 3600);
						}
					}
					return;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return QuestManager.GetQuest<IceDeityQuest>().IsActive && (spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.Player.ZoneSnow) && !NPC.AnyNPCs(ModContent.NPCType<WintrySoul>()) ? 0.00001f : 0f;
		}
		public void DoPassiveDust()
		{
			Vector2 center = NPC.Center;
			float num4 = 2.094395f;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, DustID.LavaMoss, 0.0f, 0f, 100, new Color(), 1.5f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(25, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)Main.player[NPC.target].miscCounter / 60f * 6.28318548202515 + (double)num4 * (double)index1)).ToRotationVector2() * NPC.height;
			}
		}
		public void DoExplosionDust()
        {
			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LavaMoss, 0f, -2f, 0, default, 1.1f);
				Main.dust[num].noGravity = true;
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				if (Main.dust[num].position != NPC.Center)
				{
					Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 4f;
				}
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(25, Main.LocalPlayer);
			}
		}
	}
}
