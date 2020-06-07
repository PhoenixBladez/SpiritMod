using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CobaltStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Staff");
			Tooltip.SetDefault("Summons magnetized cobalt shards at the cursor");
		}


		public override void SetDefaults()
		{
			item.damage = 39;
			item.magic = true;
			item.mana = 7;
			item.width = 40;
			item.height = 40;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CobaltStaffProj");
			item.shootSpeed = 20f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int shardType;
            shardType = Main.rand.Next(new int[] { mod.ProjectileType("CobaltStaffProj"), mod.ProjectileType("CobaltStaffProj1") });
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            int p = Terraria.Projectile.NewProjectile(mouse.X + Main.rand.Next(-20, 20), mouse.Y + Main.rand.Next(-20, 20), 0f, 0f, shardType, damage, knockBack, player.whoAmI);
            Main.projectile[p].scale = Main.rand.NextFloat(.4f, 1.1f);
            return false;
        }   
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
