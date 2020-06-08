using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod
{
    public static class GlowmaskUtils
    {
        public static void DrawNPCGlowMask(SpriteBatch spriteBatch, NPC npc, Texture2D texture) {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(
                texture,
                npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
                npc.frame,
                Color.White,
                npc.rotation,
                npc.frame.Size() / 2,
                npc.scale,
                effects,
                0
            );
        }

        public static void DrawArmorGlowMask(EquipType type, Texture2D texture, PlayerDrawInfo info) {
            switch(type) {
                case EquipType.Head: {
                        DrawData drawData = new DrawData(texture, new Vector2((int)(info.position.X - Main.screenPosition.X) + ((info.drawPlayer.width - info.drawPlayer.bodyFrame.Width) / 2), (int)(info.position.Y - Main.screenPosition.Y) + info.drawPlayer.height - info.drawPlayer.bodyFrame.Height + 4) + info.drawPlayer.headPosition + info.headOrigin, info.drawPlayer.bodyFrame, info.headGlowMaskColor, info.drawPlayer.headRotation, info.headOrigin, 1f, info.spriteEffects, 0) {
                            shader = info.headArmorShader
                        };
                        Main.playerDrawData.Add(drawData);
                    }
                    return;

                case EquipType.Body: {
                        Rectangle bodyFrame = info.drawPlayer.bodyFrame;
                        int num123 = 0;

                        bodyFrame.X += num123;
                        bodyFrame.Width -= num123;

                        if(info.drawPlayer.direction == -1) {
                            num123 = 0;
                        }

                        if(!info.drawPlayer.invis) {
                            DrawData drawData = new DrawData(texture, new Vector2((int)(info.position.X - Main.screenPosition.X - (info.drawPlayer.bodyFrame.Width / 2) + (info.drawPlayer.width / 2) + num123), ((int)(info.position.Y - Main.screenPosition.Y + info.drawPlayer.height - info.drawPlayer.bodyFrame.Height + 4))) + info.drawPlayer.bodyPosition + new Vector2(info.drawPlayer.bodyFrame.Width / 2, info.drawPlayer.bodyFrame.Height / 2), bodyFrame, info.bodyGlowMaskColor, info.drawPlayer.bodyRotation, info.bodyOrigin, 1f, info.spriteEffects, 0) {
                                shader = info.bodyArmorShader
                            };
                            Main.playerDrawData.Add(drawData);
                        }
                    }
                    return;

                case EquipType.Legs: {
                        if(info.drawPlayer.shoe != 15 || info.drawPlayer.wearsRobe) {
                            if(!info.drawPlayer.invis) {
                                DrawData drawData = new DrawData(texture, new Vector2((int)(info.position.X - Main.screenPosition.X - (info.drawPlayer.legFrame.Width / 2) + (info.drawPlayer.width / 2)), (int)(info.position.Y - Main.screenPosition.Y + info.drawPlayer.height - info.drawPlayer.legFrame.Height + 4)) + info.drawPlayer.legPosition + info.legOrigin, info.drawPlayer.legFrame, info.legGlowMaskColor, info.drawPlayer.legRotation, info.legOrigin, 1f, info.spriteEffects, 0) {
                                    shader = info.legArmorShader
                                };
                                Main.playerDrawData.Add(drawData);
                            }
                        }
                    }
                    return;
            }
        }

        public static void DrawItemGlowMask(Texture2D texture, PlayerDrawInfo info) {
            Item item = info.drawPlayer.HeldItem;
            if(info.shadow != 0f || info.drawPlayer.frozen || ((info.drawPlayer.itemAnimation <= 0 || item.useStyle == 0) && (item.holdStyle <= 0 || info.drawPlayer.pulley)) || info.drawPlayer.dead || item.noUseGraphic || (info.drawPlayer.wet && item.noWet)) {
                return;
            }

            Vector2 offset = Vector2.Zero;
            Vector2 origin = Vector2.Zero;
            float rotOffset = 0;

            if(item.useStyle == 5) {
                if(Item.staff[item.type]) {
                    rotOffset = 0.785f * info.drawPlayer.direction;
                    if(info.drawPlayer.gravDir == -1f) {
                        rotOffset -= 1.57f * info.drawPlayer.direction;
                    }

                    origin = new Vector2(texture.Width * 0.5f * (1 - info.drawPlayer.direction), (info.drawPlayer.gravDir == -1f) ? 0 : texture.Height);

                    int oldOriginX = -(int)origin.X;
                    ItemLoader.HoldoutOrigin(info.drawPlayer, ref origin);
                    offset = new Vector2(origin.X + oldOriginX, 0);
                } else {
                    offset = new Vector2(10, texture.Height / 2);
                    ItemLoader.HoldoutOffset(info.drawPlayer.gravDir, item.type, ref offset);
                    origin = new Vector2(-offset.X, texture.Height / 2);
                    if(info.drawPlayer.direction == -1) {
                        origin.X = texture.Width + offset.X;
                    }

                    offset = new Vector2(texture.Width / 2, offset.Y);
                }
            } else {
                origin = new Vector2(texture.Width * 0.5f * (1 - info.drawPlayer.direction), (info.drawPlayer.gravDir == -1f) ? 0 : texture.Height);
            }

            Main.playerDrawData.Add(new DrawData(
                texture,
                info.itemLocation - Main.screenPosition + offset,
                texture.Bounds,
                new Color(250, 250, 250, item.alpha),
                info.drawPlayer.itemRotation + rotOffset,
                origin,
                item.scale,
                info.spriteEffects,
                0
            ));
        }

        public static void DrawItemGlowMaskWorld(SpriteBatch spriteBatch, Item item, Texture2D texture, float rotation, float scale) {
            Main.spriteBatch.Draw(
                texture,
                new Vector2(item.position.X - Main.screenPosition.X + item.width / 2, item.position.Y - Main.screenPosition.Y + item.height - (texture.Height / 2) + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height),
                new Color(250, 250, 250, item.alpha),
                rotation,
                new Vector2(texture.Width / 2, texture.Height / 2),
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}

