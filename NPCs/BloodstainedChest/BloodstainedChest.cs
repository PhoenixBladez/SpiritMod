using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;

namespace SpiritMod.NPCs.BloodstainedChest
{
	[AutoloadHead]
	public class BloodstainedChest : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstained Chest");
            Main.npcFrameCount[NPC.type] = 1;

			NPCID.Sets.ActsLikeTownNPC[NPC.type] = true; //Disables happiness
		}

        public override void SetDefaults()
        {
            NPC.width = 36;
			NPC.height = 24;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 1;
            NPC.immortal = true;
            NPC.noTileCollide = false;
            NPC.dontCountMe = true;
			NPC.townNPC = true;
			NPC.friendly = true;

			for (int k = 0; k < NPC.buffImmune.Length; k++)
				NPC.buffImmune[k] = true;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            #region Shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = Color.Gold.ToVector4();
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f);
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noise").Value);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(NPC.ai[2] / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value, (NPC.Center - Main.screenPosition) + new Vector2(0, 2), null, Color.White, 0f, new Vector2(50, 50), 1.1f + (1 / 9), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
            return true;
        }

        bool rightClicked = false;

		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(rightClicked);
		public override void ReceiveExtraAI(BinaryReader reader) => rightClicked = reader.ReadBoolean();

		public override void AI()
		{
			NPC.ai[2] += 0.03f;
			//npc.homeless = true;
			Player player = Main.player[NPC.target];

			if (rightClicked)
			{
				NPC.velocity.Y = -5;
				rightClicked = false;
			}

			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldCoin, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}

			if (NPC.velocity.Y != 0)
				NPC.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
			else if (NPC.velocity == Vector2.Zero)
				NPC.rotation = 0f;

			if (NPC.ai[0] >= 1f && NPC.ai[0] <= 10f)
            {
				DoDust(NPC.ai[0]);

				NPC.ai[0]++;
				if (NPC.ai[0] == 10f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>()))
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + 355, (int)NPC.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X - 355, (int)NPC.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X - 400, (int)NPC.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + 400, (int)NPC.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
					}
				}
			}

			if (player.dead)
            {
				NPC.ai[0] = 0f;
				NPC.netUpdate = true;
            }

			if ((NPC.ai[0] > 10f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>())) || NPC.ai[3] > 0)
				NPC.ai[3]++;

			if (NPC.ai[3] == 2)
				rightClicked = true;

			if (NPC.ai[3] > 0)
            {
				DoDust(NPC.ai[3]);
				if (NPC.ai[3] <= 100 && NPC.ai[3] % 5 == 0)
				{
					int itemid;
					float val = Main.rand.NextFloat();

					if (val < .382f)
						itemid = ItemID.CopperCoin;
					else if (val < 0.83f)
						itemid = ItemID.SilverCoin;
					else if (val < 1.005f)
						itemid = ItemID.GoldCoin;
					else
						itemid = ItemID.PlatinumCoin;

					int item = Item.NewItem(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, itemid, 1);
					Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
					Main.item[item].velocity.Y /= 2;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
				}
			}
			if (NPC.ai[3] > 120)
			{
				Gore.NewGore(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, Mod.Find<ModGore>("GoldChestGore5").Type, 1f);

				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LavaMoss, 0f, -2f, 0, default, 1.1f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-30, 31) / 20) - 1.5f);
					if (dust.position != NPC.Center)
						dust.velocity = NPC.DirectionTo(Main.dust[num].position) * 4f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				}

				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 30), Vector2.Zero, ProjectileID.DD2ExplosiveTrapT2Explosion, 0, 0, NPC.target);
				SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
				NPC.DropItem(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>(), NPC.GetSource_FromAI());

				NPC.active = false;
            }
		}

		public void DoDust(float value)
        {
			Vector2 center = NPC.Center;
			float num4 = 2.094395f;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, DustID.LavaMoss, 0.0f, 0f, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * 6.28318548202515 + (double)num4 * (double)index1)).ToRotationVector2() * NPC.height;
			}
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, DustID.LavaMoss, 0.0f, 0f, 100, new Color(), 1.5f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * -6.28318548202515 + (double)num4 / 2 * (double)index1)).ToRotationVector2() * NPC.height * 1.2f;
			}
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, DustID.LavaMoss, 0.0f, 0f, 100, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * 6.28318548202515 + (double)num4 / 4 * (double)index1)).ToRotationVector2() * NPC.height * 1.4f;
			}
		}

		public override string GetChat() => "The ancient chest seems to be covered in gold, blood, and bones. It surely contains great riches, but opening it may be perilous.";
		public override void SetChatButtons(ref string button, ref string button2) => button = "Open";

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (NPC.ai[0] == 0f)
				{
					NPC.ai[0] = 1f;
					NPC.ai[3] = 1f;
				}
			}
		}

		public override List<string> SetNPCNameList() => new List<string> { "" };
	}
}
