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
			item.damage = 80;
			item.width = 74;
			item.height = 74;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.mana = 40;
			item.knockBack = 3f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<SoulDaggerProj>();
			item.UseSound = SoundID.Item44;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			Projectile.NewProjectile(position, Main.rand.NextVector2Circular(3, 3), type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}