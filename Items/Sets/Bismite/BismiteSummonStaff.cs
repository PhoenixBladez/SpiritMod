using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Bismite
{
	public class BismiteSummonStaff : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bismite Crystal Staff");
            Tooltip.SetDefault("Summons a stationary Bismite Crystal that shoots poison shards at enemies\nRight-click to cause Bismite Crystals to emit a festering wave at the cost of mana");
        }

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 12;
			item.mana = 10;
			item.width = 50;
			item.height = 50;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<BismiteSentrySummon>();
			item.shootSpeed = 0f;
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse != 2)
            {
                Vector2 mouse = Main.MouseWorld;
                float distance = Vector2.Distance(mouse, position);
                if (distance < 600f)
                {
                    Projectile.NewProjectile(mouse.X, mouse.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                    player.UpdateMaxTurrets();
                }
            }
            if (player.altFunctionUse == 2)
            {
                for (int projectileFinder = 0; projectileFinder < 200; ++projectileFinder)
                {
                    if (Main.projectile[projectileFinder].type == item.shoot && Main.projectile[projectileFinder].alpha == 0)
                    {
                        Main.projectile[projectileFinder].alpha = 240;
                    }

                }
            }
            return false;
		}
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
