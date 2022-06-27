using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.SoulDagger
{
	public class SoulDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Dagger");
			Tooltip.SetDefault("Summons a soul dagger to fight for you \nMarks enemies on attack \nMarked enemies can be attacked by other minions for a damage boost");
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.width = 74;
			Item.height = 74;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.mana = 40;
			Item.knockBack = 3f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<SoulDaggerProj>();
			Item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}