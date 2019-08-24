using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;


namespace SpiritMod.Items.Weapon.Swung
{
    public class RageBlazeDecapitator : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energized Axe");
			Tooltip.SetDefault("Every five hits on enemies, damaging granite shards are released");
		}


        public override void SetDefaults()
        {
            item.damage = 42;
            item.melee = true;
            item.width = 31;
            item.height = 25;
            item.useTime = 42;
            item.useAnimation = 42;
            item.useStyle = 1;
            item.knockBack = 11;
            item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            {

                MyPlayer gp = (MyPlayer)Main.player[Main.myPlayer].GetModPlayer(mod, "MyPlayer");
                {
                    gp.HitNumber++;
                    CombatText.NewText(new Rectangle((int)gp.player.position.X, (int)gp.player.position.Y - 60, gp.player.width, gp.player.height), new Color(29, 240, 255, 100),
                    "Hit Number: " + gp.HitNumber);
                }
            }
        }

        public override bool UseItem(Player p)
        {
            Player player = Main.player[item.owner];

            MyPlayer modPlayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
            {
                if (modPlayer.HitNumber >= 5)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float rotation = (float)(Main.rand.Next(0, 361) * (Math.PI / 180));
                        Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                        int proj = Projectile.NewProjectile(player.Center.X, player.Center.Y, velocity.X, velocity.Y, mod.ProjectileType("GraniteShard"), item.damage, item.owner, 0, 0f);
                        Main.projectile[proj].friendly = true;
                        Main.projectile[proj].hostile = false;
                        Main.projectile[proj].velocity *= 4f;
                        modPlayer.HitNumber = 0;
                    }
                }
                return true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GraniteBlock, 60);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}