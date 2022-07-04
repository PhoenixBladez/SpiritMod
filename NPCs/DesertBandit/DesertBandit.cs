using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.NPCs.DesertBandit
{
	public class DesertBandit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forsaken Bandit");
			Main.npcFrameCount[NPC.type] = 12;
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;

			//ModContent.GetInstance<SpiritMod>().NPCCandyBlacklist.Add(npc.type);
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 65;
			NPC.defense = 10;
			NPC.value = 105f;
			AIType = NPCID.Skeleton;
			NPC.knockBackResist = 0.7f;
			NPC.width = 30;
			NPC.height = 42;
			NPC.damage = 18;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.lavaImmune = true;
			for (int k = 0; k < NPC.buffImmune.Length; k++)
				NPC.buffImmune[k] = true;
			NPC.noTileCollide = false;
			NPC.alpha = 255;
			NPC.dontTakeDamage = false;
			NPC.DeathSound = SoundID.NPCDeath1;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(NPC.localAI[2]);
		public override void ReceiveExtraAI(BinaryReader reader) => NPC.localAI[2] = reader.ReadInt32();
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.localAI[2] == 0f;

		public override bool PreAI()
		{
			var textPos = new Rectangle((int)NPC.position.X, (int)NPC.position.Y - 60, NPC.width, NPC.height);

			if (NPC.alpha == 255)
			{
				for (int i = 0; i < 10; i++)
				{
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LavaMoss, 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					if (dust.position != NPC.Center)
						dust.velocity = NPC.DirectionTo(dust.position) * 4f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				}
			}

			if (NPC.alpha > 0)
				NPC.alpha -= 3;

			Player target = Main.player[NPC.target];

			NPC.TargetClosest(true);
			if (NPC.localAI[2] == 0f)
			{
				if (Vector2.Distance(target.Center, NPC.Center) > 60f)
					NPC.aiStyle = 3;
				else
					NPC.velocity.X = 0f;

				if (NPC.velocity.X < 0f)
					NPC.spriteDirection = 1;
				else if (NPC.velocity.X > 0f)
					NPC.spriteDirection = -1;

				if (NPC.velocity.X == 0f && target.dead)
					NPC.spriteDirection = 1;
			}
			else
			{
				NPC.aiStyle = 0;
				NPC.townNPC = true;
				NPC.homeless = true;

				if (NPC.localAI[2] == -1)
				{
					CombatText.NewText(textPos, new Color(61, 255, 142, 100), "Thank you again!");
					NPC.DropItem(ModContent.ItemType<Items.Sets.AccessoriesMisc.DustboundRing.Dustbound_Ring>(), NPC.GetSource_FromAI());

					for (int i = 0; i < 3; ++i)
						Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, NPC.velocity, 99);

					NPC.localAI[2] = 0f;
					NPC.active = false;
					NPC.netUpdate = true;
				}
			}

			if (NPC.CountNPCS(ModContent.NPCType<DesertBandit>()) == 1 && NPC.localAI[2] == 0f)
			{
				CombatText.NewText(textPos, new Color(61, 255, 142, 100), "Please spare me!");
				NPC.localAI[2] = 1f;
				NPC.netUpdate = true;
			}
			return true;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(ItemID.MagicLantern, 24);

		int frame = 0;

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];
			NPC.frameCounter++;
			if (NPC.localAI[2] == 0f)
			{
				if (Vector2.Distance(player.Center, NPC.Center) >= 60f)
				{
					if (NPC.frameCounter >= 7)
					{
						frame++;
						NPC.frameCounter = 0;
					}
					if (frame >= 6)
						frame = 0;
				}
				else
				{
					if (NPC.frameCounter >= 5)
					{
						frame++;
						NPC.frameCounter = 0;
					}
					if (frame >= 11)
						frame = 6;

					if (frame < 6)
						frame = 6;

					if (frame == 9 && NPC.frameCounter == 4 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
						player.Hurt(PlayerDeathReason.LegacyDefault(), NPC.damage * 2, NPC.direction * -1, false, false, false, -1);
				}
			}
			NPC.frame.Y = frameHeight * frame;
		}

		public override bool CanChat() => NPC.localAI[2] != 0f;
		public override string GetChat() => "Please, spare me! I was so desperate...I haven't had food for days. You can leave me be if you want, but if you give me that crown, I'll give you what I have and be on my way. Promise.";
		public override void SetChatButtons(ref string button, ref string button2) => button = "Spare";

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (Main.npcChatText == TravelingMerchantDesertQuest.ThankText)
				{
					NPC.localAI[2] = -1;

					for (int i = 0; i < Main.LocalPlayer.inventory.Length; ++i)
					{
						Item item = Main.LocalPlayer.inventory[i];
						if (item.type == ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>() && !item.IsAir)
							item.TurnToAir();
					}
				}
				else
					Main.npcChatText = "Either leave me be or hand me the crown. It's up to you.";
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.UnusedBrown, hitDirection, -1f, 1, default, .61f);
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DesertBanditGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DesertBanditGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DesertBanditGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DesertBanditGore2").Type, 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			SoundEngine.PlaySound(SoundID.Coins);
			int num1 = 0;
			for (int index = 0; index < 59; ++index)
			{
				if (target.inventory[index].type >= ItemID.CopperCoin && target.inventory[index].type <= ItemID.PlatinumCoin)
				{
					int number = Item.NewItem(NPC.GetSource_Loot("Steal"), (int)target.position.X, (int)target.position.Y, target.width, target.height, target.inventory[index].type, 1, false, 0, false, false);
					int num2 = target.inventory[index].stack / 8;
					if (Main.expertMode)
						num2 = (int)(target.inventory[index].stack * 0.2);
					int num3 = target.inventory[index].stack - num2;
					target.inventory[index].stack -= num3;
					if (target.inventory[index].type == ItemID.CopperCoin)
						num1 += num3;
					if (target.inventory[index].type == ItemID.SilverCoin)
						num1 += num3 * 100;
					if (target.inventory[index].type == ItemID.GoldCoin)
						num1 += num3 * 10000;
					if (target.inventory[index].type == ItemID.PlatinumCoin)
						num1 += num3 * 1000000;
					if (target.inventory[index].stack <= 0)
						target.inventory[index] = new Item();
					Main.item[number].stack = num3;
					Main.item[number].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
					Main.item[number].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
					Main.item[number].noGrabDelay = 100;
					if (index == 58)
						Main.mouseItem = target.inventory[index].Clone();
				}
			}
			target.lostCoins = num1;
			target.lostCoinString = Main.ValueToCoins(target.lostCoins);
		}
	}
}