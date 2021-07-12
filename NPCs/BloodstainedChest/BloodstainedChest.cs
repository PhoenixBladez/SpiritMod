using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;

namespace SpiritMod.NPCs.BloodstainedChest
{
	[AutoloadHead]
	public class BloodstainedChest : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstained Chest");
            Main.npcFrameCount[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.width = 36;
			npc.height = 24;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.immortal = true;
            npc.noTileCollide = false;
            npc.dontCountMe = true;
			npc.townNPC = true;
        }
        float alphaCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = Color.Gold.ToVector4();
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f);
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(npc.ai[2] / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), null, Color.White, 0f, new Vector2(50, 50), 1.1f + (1 / 9), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
            return true;
        }
        bool rightClicked = false;
		public override void AI()
		{
			npc.ai[2] += 0.03f;
			npc.homeless = true;
			Player player = Main.player[npc.target];
			if (rightClicked)
			{
				npc.velocity.Y = -5;
				rightClicked = false;
			}
			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 246, 0, 0);
				Main.dust[dust].velocity = Vector2.Zero;
			}
			if (npc.velocity.Y != 0)
			{
				npc.rotation += Main.rand.NextFloat(-0.1f, 0.1f);
			}
			if (npc.velocity == Vector2.Zero)
            {
				npc.rotation = 0f;
            }
			if (npc.ai[0] >= 1f && npc.ai[0] <= 10f)
            {
				DoDust(npc.ai[0]);
				npc.ai[0]++;
				if (npc.ai[0] == 10f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>()))
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						NPC.NewNPC((int)npc.position.X + 355, (int)npc.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC((int)npc.position.X - 355, (int)npc.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC((int)npc.position.X - 400, (int)npc.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());
						NPC.NewNPC((int)npc.position.X + 400, (int)npc.Center.Y - Main.rand.Next(30, 50), ModContent.NPCType<NPCs.DesertBandit.DesertBandit>());

					}
				}
			}
			if (player.dead)
            {
				npc.ai[0] = 0f;
				npc.netUpdate = true;
            }
			if ((npc.ai[0] > 10f && !NPC.AnyNPCs(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>())) || npc.ai[3] > 0)
            {
				npc.ai[3]++;
			}
			if (npc.ai[3] == 2)
            {
				rightClicked = true;
			}
			if (npc.ai[3] > 0)
            {
				DoDust(npc.ai[3]);
				if (npc.ai[3] <= 100 && npc.ai[3] % 5 == 0)
				{
					int itemid;
					int item = 0;
					float val = Main.rand.NextFloat();
					if (val < .382f)
					{
						itemid = ItemID.CopperCoin;
					}
					else if (val < 0.83f)
					{
						itemid = ItemID.SilverCoin;
					}
					else if (val < 1.005f)
					{
						itemid = ItemID.GoldCoin;
					}
					else
					{
						itemid = ItemID.PlatinumCoin;
					}

					item = Item.NewItem(npc.Center, Vector2.Zero, itemid, 1);
					Main.item[item].velocity = Vector2.UnitY.RotatedBy(Main.rand.NextFloat(1.57f, 4.71f)) * 4;
					Main.item[item].velocity.Y /= 2;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item);
				}
			}
			if (npc.ai[3] > 120)
			{
				Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore5"), 1f);
				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 258, 0f, -2f, 0, default(Color), 1.1f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center)
					{
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 4f;
					}
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				}
				Projectile.NewProjectile(npc.Center - new Vector2(0, 30), Vector2.Zero, ProjectileID.DD2ExplosiveTrapT2Explosion, 0, 0, npc.target);
				Main.PlaySound(SoundID.Item14, npc.Center);
				npc.DropItem(ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.RoyalCrown>());

				npc.active = false;
            }
		}
		public void DoDust(float value)
        {
			Vector2 center = npc.Center;
			float num4 = 2.094395f;
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), 1f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * 6.28318548202515 + (double)num4 * (double)index1)).ToRotationVector2() * npc.height;
			}
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), 1.5f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * -6.28318548202515 + (double)num4 / 2 * (double)index1)).ToRotationVector2() * npc.height * 1.2f;
			}
			for (int index1 = 0; index1 < 3; ++index1)
			{
				int index2 = Dust.NewDust(center, 0, 0, 258, 0.0f, 0f, 100, new Color(), 2f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].noLight = true;
				Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(13, Main.LocalPlayer);
				Main.dust[index2].position = center + ((float)((double)(float)value / 60f * 6.28318548202515 + (double)num4 / 4 * (double)index1)).ToRotationVector2() * npc.height * 1.4f;
			}
		}
		public override string TownNPCName()
		{
			string[] names = { "Bloodstained Chest" };
			return Main.rand.Next(names);
		}
		public override string GetChat()
		{
			return "The ancient chest seems to be covered in gold, blood, and bones. It surely contains great riches, but opening it may be perilous.";
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Open";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (npc.ai[0] == 0f)
                {
					npc.ai[0] = 1f;
					rightClicked = true;
                }
				else if (npc.ai[0] > 10f && NPC.CountNPCS(ModContent.NPCType<NPCs.DesertBandit.DesertBandit>()) == 1)
                {
					npc.ai[3] = 1f;
                }
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!Mechanics.QuestSystem.QuestManager.GetQuest<Mechanics.QuestSystem.Quests.TravelingMerchantDesertQuest>().IsActive)
				return 0f;

			return spawnInfo.player.ZoneDesert && !NPC.AnyNPCs(ModContent.NPCType<BloodstainedChest>()) && spawnInfo.spawnTileY < Main.rockLayer ? 0.55f : 0f;
		}
	}
}
