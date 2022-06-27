using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarChestLoot
{
	public class ReachBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarheart Boomerang");
			Tooltip.SetDefault("Shoots out two boomerangs on use");
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 37;
			Item.useAnimation = 37;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 0, 4, 0);
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Returning.ReachBoomerang>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
		}

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; ++i)
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                    return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(Item.GetSource_ItemUse(Item), position.X, position.Y, velocity.X + ((float)Main.rand.Next(-200, 200) / 100), velocity.Y + ((float)Main.rand.Next(-200, 200) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
			return true;
		}
	}
}