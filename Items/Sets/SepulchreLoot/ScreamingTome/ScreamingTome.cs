using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.ScreamingTome
{
    public class ScreamingTome : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screaming Tome");
			Tooltip.SetDefault("Creates orbiting skulls\nRelease to launch skulls");

			Item.staff[Item.type] = false;
		}

		public override void SetDefaults()
        {
            Item.damage = 16;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.width = 36;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<ScreamingSkull>();
            Item.shootSpeed = 18f;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item104;
            Item.value = Item.buyPrice(0, 1, 40, 0);
            Item.useTurn = false;
            Item.mana = 8;
            Item.channel = true;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ScreamingSkull>()] < 4;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            int p = Projectile.NewProjectile(player.Center.X - Main.rand.Next(-50, 50), player.Center.Y - Main.rand.Next(-50, 50), 0f, 0f, type, damage, knockback, player.whoAmI);
            Main.projectile[p].ai[0] = player.whoAmI;
            return false;
        }
    }
}
