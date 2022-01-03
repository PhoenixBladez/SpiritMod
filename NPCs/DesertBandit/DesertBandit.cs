using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace SpiritMod.NPCs.DesertBandit
{
	public class DesertBandit : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forsaken Bandit");
			Main.npcFrameCount[npc.type] = 12;
			NPCID.Sets.TrailCacheLength[npc.type] = 20;
			NPCID.Sets.TrailingMode[npc.type] = 0;

			//ModContent.GetInstance<SpiritMod>().NPCCandyBlacklist.Add(npc.type);
		}

		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 65;
			npc.defense = 10;
			npc.value = 105f;
			aiType = NPCID.Skeleton;
			npc.knockBackResist = 0.7f;
			npc.width = 30;
			npc.height = 42;
			npc.damage = 18;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.lavaImmune = true;
			for (int k = 0; k < npc.buffImmune.Length; k++)
				npc.buffImmune[k] = true;
			npc.noTileCollide = false;
			npc.alpha = 255;
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 1);
        }

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(npc.localAI[2]);
		public override void ReceiveExtraAI(BinaryReader reader) => npc.localAI[2] = reader.ReadInt32();

		public override bool PreAI()
		{
			var textPos = new Rectangle((int)npc.position.X, (int)npc.position.Y - 60, npc.width, npc.height);

			if (npc.alpha == 255)
            {
				for (int i = 0; i < 10; i++)
				{
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.LavaMoss, 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					if (dust.position != npc.Center)
						dust.velocity = npc.DirectionTo(dust.position) * 4f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				}
			}
			if (npc.alpha > 0)
				npc.alpha -= 3;
			Player target = Main.player[npc.target];

			npc.TargetClosest(true);
			if (npc.localAI[2] == 0f)
			{
				if (Vector2.Distance(target.Center, npc.Center) > 60f)
					npc.aiStyle = 3;
				else
					npc.velocity.X = 0f;

				if (npc.velocity.X < 0f)
					npc.spriteDirection = 1;
				else if (npc.velocity.X > 0f)
					npc.spriteDirection = -1;

				if (npc.velocity.X == 0f && target.dead)
					npc.spriteDirection = 1;
			}
			else
            {
				npc.aiStyle = 0;
				npc.friendly = true;
				npc.townNPC = true;
				npc.homeless = true;
				npc.immortal = true;
				if (Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.TravelingMerchantDesertQuest>().IsCompleted)
				{
					CombatText.NewText(textPos, new Color(61, 255, 142, 100), "Thank you again!");
					npc.active = false;
					Gore.NewGore(npc.position, npc.velocity, 99);
					Gore.NewGore(npc.position, npc.velocity, 99);
					Gore.NewGore(npc.position, npc.velocity, 99);
					npc.localAI[2] = 0f;
					npc.netUpdate = true;
				}
			}
			if (NPC.CountNPCS(ModContent.NPCType<DesertBandit>()) == 1 && npc.localAI[2] == 0f)
            {
				CombatText.NewText(textPos, new Color(61, 255, 142, 100), "Please spare me!");
				npc.localAI[2] = 1f;
				npc.netUpdate = true;
            }
			return true;
		}

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(24))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MagicLantern);
        }

		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.localAI[2] == 0f)
			{
				if (Vector2.Distance(player.Center, npc.Center) >= 60f)
				{
					if (npc.frameCounter >= 7)
					{
						frame++;
						npc.frameCounter = 0;
					}
					if (frame >= 6)
						frame = 0;
				}
				else
				{
					if (npc.frameCounter >= 5)
					{
						frame++;
						npc.frameCounter = 0;
					}
					if (frame >= 11)
						frame = 6;

					if (frame < 6)
						frame = 6;

					if (frame == 9 && npc.frameCounter == 4 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
						player.Hurt(PlayerDeathReason.LegacyDefault(), npc.damage * 2, npc.direction * -1, false, false, false, -1);
				}
			}
			npc.frame.Y = frameHeight * frame;
		}

		//also potentially breaking [!] - gabe
		public override bool CanChat() => npc.localAI[2] != 0f;

		public override string GetChat() => "Please, spare me! Our group only attacked because we need these artifacts to get enough money to feed our families! I know you have no reason to trust me, but if you could open that chest and give me the Royal Crown, I'll give you all I have.";
		public override void SetChatButtons(ref string button, ref string button2) => button = "Kill";

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				npc.StrikeNPC(200, 0f, 0);
				npc.netUpdate = true;

				npc.life = -1;
				npc.checkDead();
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.UnusedBrown, hitDirection, -1f, 1, default, .61f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DesertBandit/DesertBanditGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DesertBandit/DesertBanditGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DesertBandit/DesertBanditGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DesertBandit/DesertBanditGore2"), 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(18, 0));
			int num1 = 0;
			for (int index = 0; index < 59; ++index)
			{
				if (target.inventory[index].type >= ItemID.CopperCoin && target.inventory[index].type <= ItemID.PlatinumCoin)
				{
					int number = Item.NewItem((int) target.position.X, (int) target.position.Y, target.width, target.height, target.inventory[index].type, 1, false, 0, false, false);
					int num2 = target.inventory[index].stack / 8;
					if (Main.expertMode)
						num2 = (int) (target.inventory[index].stack * 0.2);
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