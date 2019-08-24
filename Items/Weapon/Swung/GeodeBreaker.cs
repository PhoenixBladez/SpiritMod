using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GeodeBreaker : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Breaker");
				Tooltip.SetDefault("Shoots out a geode that splits into crystal shards");
	
		}


        public override void SetDefaults()
        {
            item.damage = 35;
            item.melee = true;
            item.width = 36;
            item.height = 36;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 0, 70, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 6;
			item.shoot = mod.ProjectileType("GeodeShards");
			item.shootSpeed = 6f;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            {
                if (crit)
                {
                    target.AddBuff(BuffID.CursedInferno, 240, true);
                    target.AddBuff(BuffID.Frostburn, 240, true);
                    target.AddBuff(BuffID.OnFire, 240, true);
                }
            }
        }
    }
}
