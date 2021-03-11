using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Items.GamblerChestLoot.Jem;
using SpiritMod.Items.GamblerChestLoot.FunnyFirework;
using SpiritMod.Items.GamblerChestLoot.Champagne;
using SpiritMod.Items.GamblerChestLoot.GildedMustache;
using SpiritMod.Mechanics.Fathomless_Chest;

namespace SpiritMod.Items.Consumable.GamblerChests.GamblerChestNPCs
{
    internal class GoldChestBottom : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chest");
            Main.npcFrameCount[npc.type] = 2;
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
        }
        int counter = -1;
        float alphaCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 colorMod = Color.Gold.ToVector4();
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sineAdd / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f + (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), null, Color.White, 0f, new Vector2(50, 50), 1.1f + (sineAdd / 9), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion

            if (counter > 0)
            {
                Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49_Top"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), null, new Color(200, 200, 200, 0), 0f, new Vector2(50, 50), 0.33f, SpriteEffects.None, 0f);
            }
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
            Main.spriteBatch.Draw(mod.GetTexture("Items/Consumable/GamblerChests/GamblerChestNPCs/GoldChestBottom_White"), (npc.Center - Main.screenPosition) + new Vector2(0, 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * alpha, npc.rotation, center, npc.scale, SpriteEffects.None, 0f);
            }
        }
        bool rightClicked = false;
         float sineAdd = -1;
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
               int dust = Dust.NewDust(npc.position, npc.width, npc.height, 246, 0, 0);
               Main.dust[dust].velocity = Vector2.Zero;
            }
            if (rightClicked && npc.velocity.Y != 0)
            {
                npc.rotation += Main.rand.NextFloat(-0.1f,0.1f);
            }
            counter--;
            if (counter == 0)
            {
                npc.active = false;
                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore4"), 1f);
                Gore.NewGore(npc.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore5"), 1f);
                Projectile.NewProjectile(npc.Center - new Vector2(0, 30), Vector2.Zero, 695, 0, 0, npc.target);
            }
            if (counter > 0)
            {
                if (counter <= 100 && counter % 5 == 0)
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
                    else if (val < 0.999f)
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
                }

				if (counter == 50) {
					npc.DropItem(ModContent.ItemType<Jem>(), 0.01f);
                    npc.DropItem(ModContent.ItemType<Items.Consumable.Food.GoldenCaviar>(), 0.1f);
					npc.DropItem(ModContent.ItemType<FunnyFirework>(), 0.08f, Main.rand.Next(5, 9));
					npc.DropItem(ItemID.AngelStatue, 0.02f);
					npc.DropItem(ModContent.ItemType<Champagne>(), 0.08f, Main.rand.Next(1, 3));
                     npc.DropItem(ModContent.ItemType<Mystical_Dice>(), 0.05f);
					switch (Main.rand.NextBool()) { //mutually exclusive
						case true:
							npc.DropItem(ModContent.ItemType<GildedMustache>(), 0.05f);
							break;
						case false:
							npc.DropItem(ModContent.ItemType<GildedMustache>(), 0.05f); //replace with staff
							break;
					}
				}
			}
            if (rightClicked && npc.velocity.Y == 0 && counter < 0)
                {
                    npc.rotation = 0;
                    npc.frame.Y = npc.height;
                    counter = 200;
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<GoldChestTop>(), 0, 0, npc.target, npc.position.X, npc.position.Y);
                }
        }
    }
    internal class GoldChestTop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Chest");
            Main.projFrames[projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.magic = true;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.timeLeft = 200;
        }
        public override void AI()
        {
            projectile.velocity.Y = -0.05f;
            projectile.velocity.X = 0;
            projectile.frameCounter++;
            if (projectile.frameCounter > Math.Sqrt(Math.Max((projectile.timeLeft - 100), 1) / 2) / 2)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 12)
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.timeLeft == 1)
            {
                Gore.NewGore(projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore1"), 1f);
                Gore.NewGore(projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore2"), 1f);
                Gore.NewGore(projectile.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * 7, mod.GetGoreSlot("Gores/GamblerChests/GoldChestGore3"), 1f);
            }
        }
         public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Color.White;
            float alpha = (Math.Max(0, 50 - projectile.timeLeft)) / 50f;
            Main.spriteBatch.Draw(mod.GetTexture("Items/Consumable/GamblerChests/GamblerChestNPCs/GoldChestTop_White"), (projectile.position - Main.screenPosition), new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), color * alpha, projectile.rotation, Vector2.Zero, projectile.scale, SpriteEffects.None, 0f);
        }
    }
}
