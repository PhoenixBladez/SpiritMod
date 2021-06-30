using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireLamp : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 40;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.magic = true;
            item.width = 36;
            item.height = 40;
            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = ModContent.ProjectileType<StarfireProj>();
            item.shootSpeed = 14f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item45;
            item.value = Item.sellPrice(silver: 55);
            item.useTurn = false;
            item.mana = 15;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Lamp");
            Tooltip.SetDefault("Creates homing starfires");
        }

        public override void UseStyle(Player player)
        {
            Vector2 vector2_1 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
                vector2_1.X = (float)player.bodyFrame.Width - vector2_1.X;
            if ((double)player.gravDir != 1.0)
                vector2_1.Y = (float)player.bodyFrame.Height - vector2_1.Y;
            vector2_1 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            Vector2 vector2_2 = player.RotatedRelativePoint(player.position + vector2_1, true) - player.velocity;
            for (int index = 0; index < 4; ++index)
            {
                Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, 112, (float)(player.direction * 2), 0.0f, 150, new Color(), 1f)];
                dust.position = vector2_2;
                dust.velocity *= 0.0f;
                dust.noGravity = true;
                dust.fadeIn = 0.7f;
                dust.velocity += new Vector2(0f, -3f);
                if (Main.rand.Next(2) == 0)
                    dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
                dust.scale = 0.5f;
                if (Main.rand.Next(2) == 0)
                    dust.customData = (object)player;
            }
        }

        public override void HoldItem(Player player)
        {
            StarfireLampPlayer.isHolding = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position.Y -= 45;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "Starjinx", 10);
            recipe.AddIngredient(ItemID.FallenStar, 6);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class StarfireLampPlayer : ModPlayer
    {
        public static bool isHolding = false;
        public override void ResetEffects()
        {
            isHolding = false;
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int body = layers.FindIndex(l => l == PlayerLayer.Body);
            if (body < 0)
                return;

            layers.Insert(body - 1, new PlayerLayer(mod.Name, "Body",
                delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }

                if (isHolding)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    Mod mod = ModLoader.GetMod("Starjinx");
                    StarfireLampPlayer modPlayer = drawPlayer.GetModPlayer<StarfireLampPlayer>();
                    Vector2 Position = drawPlayer.position;
                    DrawData drawData = new DrawData();
                    SpriteEffects spriteEffects;
                    SpriteEffects effect;
                    if ((double)drawPlayer.gravDir == 1.0)
                    {
                        if (drawPlayer.direction == 1)
                        {
                            spriteEffects = SpriteEffects.None;
                            effect = SpriteEffects.None;
                        }
                        else
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally;
                            effect = SpriteEffects.FlipHorizontally;
                        }
                    }
                    else
                    {
                        if (drawPlayer.direction == 1)
                        {
                            spriteEffects = SpriteEffects.FlipVertically;
                            effect = SpriteEffects.FlipVertically;
                        }
                        else
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                            effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                        }
                    }

                    //change
                    Microsoft.Xna.Framework.Color color12 = drawPlayer.GetImmuneAlphaPure(Lighting.GetColor((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, Microsoft.Xna.Framework.Color.White), drawInfo.shadow);
                    Microsoft.Xna.Framework.Color color19 = Microsoft.Xna.Framework.Color.Lerp(color12, Microsoft.Xna.Framework.Color.White, 0.7f);
                    Texture2D texture2D = ModContent.GetTexture("SpiritMod/Items/Sets/StarjinxSet/StarfireLamp/StarfireLamp");
                    Texture2D texture = ModContent.GetTexture("SpiritMod/Items/Sets/StarjinxSet/StarfireLamp/StarfireLampGlow");
                    int num23 = !drawPlayer.setForbiddenCooldownLocked ? 1 : 0;
                    int num24 = (int)((double)((float)((double)drawPlayer.miscCounter / 300.0 * 6.28318548202515)).ToRotationVector2().Y * 6.0);
                    float num25 = ((float)((double)drawPlayer.miscCounter / 75.0 * 6.28318548202515)).ToRotationVector2().X * 4f;
                    Microsoft.Xna.Framework.Color color20 = new Microsoft.Xna.Framework.Color(80, 70, 40, 0) * (float)((double)num25 / 8.0 + 0.5) * 0.8f;
                    if (num23 == 0)
                    {
                        num24 = 0;
                        num25 = 2f;
                        color20 = new Microsoft.Xna.Framework.Color(80, 70, 40, 0) * 0.3f;
                        color19 = color19.MultiplyRGB(new Microsoft.Xna.Framework.Color(0.5f, 0.5f, 1f));
                    }
                    Vector2 position = new Vector2((float)(int)((double)Position.X - (double)Main.screenPosition.X - (double)(drawPlayer.bodyFrame.Width / 2) + (double)(drawPlayer.width / 2)), (float)(int)((double)Position.Y - (double)Main.screenPosition.Y + (double)drawPlayer.height - (double)drawPlayer.bodyFrame.Height + 4.0)) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)) + new Vector2((float)(-drawPlayer.direction), (float)(num24 - 60));
                    drawData = new DrawData(texture2D, position, new Microsoft.Xna.Framework.Rectangle?(), color12, drawPlayer.bodyRotation, texture2D.Size() / 2f, 1f, spriteEffects, 0);
                    Main.playerDrawData.Add(drawData);
                    for (float num26 = 0.0f; (double)num26 < 8.0; ++num26)
                    {
                        drawData = new DrawData(texture, position + (num26 * 1.570796f).ToRotationVector2() * num25, new Microsoft.Xna.Framework.Rectangle?(), color20, drawPlayer.bodyRotation, texture2D.Size() / 2f, 1f, spriteEffects, 0);
                        Main.playerDrawData.Add(drawData);
                    }
                }
            }));
        }
    }
}
