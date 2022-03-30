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

			Item.staff[item.type] = false;
		}

		public override void SetDefaults()
        {
            item.damage = 16;
            item.noMelee = true;
            item.magic = true;
            item.width = 36;
            item.height = 40;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<ScreamingSkull>();
            item.shootSpeed = 18f;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item104;
            item.value = Item.buyPrice(0, 1, 40, 0);
            item.useTurn = false;
            item.mana = 8;
            item.channel = true;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ScreamingSkull>()] < 4;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Projectile.NewProjectile(player.Center.X - Main.rand.Next(-50, 50), player.Center.Y - Main.rand.Next(-50, 50), 0f, 0f, type, damage, knockBack, player.whoAmI);
            Main.projectile[p].ai[0] = player.whoAmI;
            return false;
        }
    }
}
