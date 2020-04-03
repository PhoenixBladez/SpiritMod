using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class ShroomFishSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fungal Icthyoid");
            Tooltip.SetDefault("Summons slow, homing spores around the player\nThese spores do not take up minion slots");
        }



        public override void SetDefaults()
        {
            item.damage = 10;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.summon = true;
            item.width = 32;
            item.height = 42;
            item.useTime = 25;
            item.mana = 14;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 65, 0);
            item.rare = 2;
            item.autoReuse = false;
            item.shootSpeed = 9;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("ShroomSummon");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int dimension = 24;
            int dimension2 = 90;
            for (int j = 0; j < 50; j++)
            {
                int randValue = Main.rand.Next(200 - j * 2, 400 + j * 2);
                Vector2 center = player.Center;
                center.X += Main.rand.Next(-randValue, randValue + 1);
                center.Y += Main.rand.Next(-randValue, randValue + 1);

                if (!Collision.SolidCollision(center, dimension, dimension) && !Collision.WetCollision(center, dimension, dimension))
                {
                    center.X += dimension / 2;
                    center.Y += dimension / 2;

                    if (Collision.CanHit(new Vector2(player.Center.X, player.position.Y), 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(player.Center.X, player.position.Y - 50f), 1, 1, center, 1, 1))
                    {
                        int x = (int)center.X / 16;
                        int y = (int)center.Y / 16;

                        bool flag = false;
                        if (Main.rand.Next(4) == 0 && Main.tile[x, y] != null && Main.tile[x, y].wall > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            center.X -= dimension2 / 2;
                            center.Y -= dimension2 / 2;

                            if (Collision.SolidCollision(center, dimension2, dimension2))
                            {
                                center.X += dimension2 / 2;
                                center.Y += dimension2 / 2;
                                flag = true;
                            }
                        }

                        if (flag)
                        {
                            for (int k = 0; k < 1000; k++)
                            {
                                if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == mod.ProjectileType("ShroomSummon") && (center - Main.projectile[k].Center).Length() < 48f)
                                {
                                    flag = false;
                                    break;
                                }
                            }

                            if (flag && Main.myPlayer == player.whoAmI)
                            {
                                Projectile.NewProjectile(center.X, center.Y, 0f, 0f, mod.ProjectileType("ShroomSummon"), damage, knockBack, player.whoAmI, 0f, 0f);
                            }
                        }
                    }
                }
            }
            return false;
        }
    }

}
