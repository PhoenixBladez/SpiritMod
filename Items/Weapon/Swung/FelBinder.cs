using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class FelBinder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fel Binder");
			Tooltip.SetDefault("Fires cursed souls at foes");
		}


		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 8;
			item.value = Terraria.Item.sellPrice(0, 7, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<FelShot>();
			item.shootSpeed = 6f;
			item.autoReuse = true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if(Main.rand.Next(2) == 0) {
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 75);
			}
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			{
				target.AddBuff(ModContent.BuffType<FelBrand>(), 200);
			}
		}
	}
}