using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Items.Sets.GamblerChestLoot.Jem;
using SpiritMod.Items.Sets.GamblerChestLoot.FunnyFirework;
using SpiritMod.Items.Sets.GamblerChestLoot.GildedMustache;
using SpiritMod.Items.Sets.GamblerChestLoot.Champagne;
using SpiritMod.Mechanics.Fathomless_Chest;
using SpiritMod.Items.Sets.GamblerChestLoot.RegalCane;
using Terraria.Audio;

namespace SpiritMod.Items.Sets.GamblerChestLoot.GamblerChestNPCs
{
    internal class PlatinumChestBottom : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Chest");
            Main.npcFrameCount[npc.type] = 9;
        }
        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 34;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 1;
            npc.immortal = true;
            npc.noTileCollide = false;
            npc.dontCountMe = true;
        }
        int counter = -1;
        float alphaCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = Color.Silver.ToVector4();
			SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sineAdd / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), null, Color.White, 0f, new Vector2(50, 50), 1.1f + (sineAdd / 9), SpriteEffects.None, 0f);
            if (counter > 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                float breakCounter = 1 - (counter / 200f);
				SpiritMod.CircleNoise.Parameters["breakCounter"].SetValue(breakCounter);
				SpiritMod.CircleNoise.Parameters["rotation"].SetValue(breakCounter);
				SpiritMod.CircleNoise.Parameters["colorMod"].SetValue(colorMod);
				SpiritMod.CircleNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
				SpiritMod.CircleNoise.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0,2), null, Color.White, 0f, new Vector2(50, 50), 3 + (breakCounter / 2), SpriteEffects.None, 0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
             Main.spriteBatch.Draw(Main.npcTexture[npc.type], (npc.Center - Main.screenPosition) + new Vector2(0, 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), lightColor, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            if (counter > 0)
            {
            Color color = Color.White;
            float alpha = (Math.Max(0, 50 - counter)) / 50f;
            Main.spriteBatch.Draw(mod.GetTexture("Items/Sets/GamblerChestLoot/GamblerChestNPCs/PlatinumChestBottom_White"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * alpha, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
            }
        }
        bool rightClicked = false;
         float sineAdd = -1;
         int frameCounter = 0;
         int frame;
        public override void AI()
        {
             sineAdd += 0.03f;
            if (!rightClicked && npc.velocity.Y == 0 && counter < -50)
            {
                rightClicked = true;
                npc.velocity.Y = -5;
            }
            if (counter % 10 == 0)
            {
               int dust = Dust.NewDust(npc.position, npc.width, npc.height, 247, 0, 0);
               Main.dust[dust].velocity = Vector2.Zero;
            }
            if (rightClicked && npc.velocity.Y != 0 && counter < 0)
            {
                npc.rotation += Main.rand.NextFloat(-0.1f,0.1f);
            }
			if(rightClicked && npc.velocity.Y == 0 && npc.localAI[0] == 0) {
				npc.localAI[0]++;
				Main.PlaySound(SoundID.Dig, npc.Center);
			}
            counter--;
            if (counter == 0)
            {
                 for (int i = 0; i < 30; i++)
                {
                    int itemid;
                    int item = 0;
                    float val = Main.rand.NextFloat();
                    if (val < .214f)
                    {
                        itemid = ItemID.CopperCoin;
                    }
                    else if (val < .214f + .366f)
                    {
                        itemid = ItemID.SilverCoin;
                    }
                    else if (val < 0.991f)
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

                npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.Jem.Jem>(), 0.02f);
                npc.DropItem(ModContent.ItemType<Items.Consumable.Food.GoldenCaviar>(), 0.15f);
				npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.FunnyFirework.FunnyFirework>(), 0.1f, Main.rand.Next(5, 9));
				npc.DropItem(ItemID.AngelStatue, 0.01f);
				npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.Champagne.Champagne>(), 0.1f, Main.rand.Next(1, 3));
                    npc.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.06f);
				switch (Main.rand.NextBool()) { //mutually exclusive
					case true:
						npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.GildedMustache.GildedMustache>(), 0.08f);
						break;
					case false:
						npc.DropItem(ModContent.ItemType<Items.Sets.GamblerChestLoot.RegalCane.RegalCane>(), 0.08f);
						break;
				}
                npc.active = false;
				Main.PlaySound(SoundID.Item14, npc.Center);

                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/PlatinumChestGore1"), 1f);
                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/PlatinumChestGore2"), 1f);
                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/PlatinumChestGore3"), 1f);
                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/PlatinumChestGore4"), 1f);
            }
            if (counter > 0)
            {
                frameCounter++;
                if (frameCounter > Math.Sqrt(Math.Max((counter - 100), 1) / 2))
                {
                    frameCounter = 0;
                    frame++;
                    if (frame >= 9)
                    {
                        frame = 0;
                    }
                    npc.frame.Y = frame * npc.height;
                }
                if (counter % 10 == 0 && counter < 70)
                {
                    int proj = Projectile.NewProjectile(npc.Center + new Vector2(Main.rand.Next(-20,20), Main.rand.Next(-20,20)), Vector2.Zero, ProjectileID.LunarFlare, 0, 0, npc.target);
                    Main.projectile[proj].timeLeft = 2;
				}
            }
            if (rightClicked && npc.velocity.Y == 0 && counter < 0)
                {
                    npc.rotation = 0;
                    counter = 250;
                    npc.noGravity = true;
                    npc.velocity.Y = -0.09f;
                }
        }
    }
}
