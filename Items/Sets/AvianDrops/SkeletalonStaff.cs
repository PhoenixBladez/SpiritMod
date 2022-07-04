using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.AvianDrops
{
	public class SkeletalonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletalon Staff");
			Tooltip.SetDefault("Summons an army of fossilized birds to fight for you");
		}


		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 3, 25, 0);
			Item.rare = ItemRarityID.Green;
			Item.mana = 12;
			Item.damage = 13;
			Item.knockBack = 3;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<SkeletalonMinion>();
			Item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				player.MinionNPCTargetAim(true);
			return player.altFunctionUse == 2 ? null : false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i <= Main.rand.Next(0, 3); i++) {
				int proj = Projectile.NewProjectile(source, position.X + Main.rand.Next(-30, 30), position.Y, 0f, 0f, type, damage, knockback, player.whoAmI);
				Projectile projectile = Main.projectile[proj];
				for (int j = 0; j < 10; j++) {
					int d = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Dirt, (float)(Main.rand.Next(5) - 2), (float)(Main.rand.Next(5) - 2), 133);
					Main.dust[d].scale *= .75f;
				}
			}
			return false;
		}
	}
}