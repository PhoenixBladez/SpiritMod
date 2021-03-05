using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.BaseProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow.AdornedBow
{
	public class ScarabBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adorned Bow");
			Tooltip.SetDefault("Hold longer for more damage\nConverts wooden arrows into piercing adorned arrows, that get enchanted upon full charge");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}
		public override void SetDefaults()
		{
			item.damage = 24;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 46;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;
			item.autoReuse = false;
			item.shootSpeed = 6.5f;
			item.crit = 8;
			item.reuseDelay = 20;
			item.channel = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<ScarabArrow>();
			}
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<AdornedBowProj>(), damage - item.damage, knockBack, player.whoAmI, type);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow"), rotation, scale);
	}
	public class AdornedBowProj : ChargeBowProj
	{
		public override string Texture => "SpiritMod/Items/Weapon/Bow/AdornedBow/ScarabBow";

		protected override void SetBowDefaults()
		{
			minDamage = 8;
			maxDamage = 40;
			minVelocity = 10;
			maxVelocity = 18;
			predictor = -1;
			chargeRate = 0.02f;
			dechargeRate = 2;
		}

		protected override void Shoot(bool firstFire) 
		{
			if (!firstFire)
				return;
			if (Main.myPlayer == projectile.owner) {
				Projectile proj = Main.projectile[CreateArrow()];
				if (charge >= 1 && projectile.ai[0] == ModContent.ProjectileType<ScarabArrow>()) proj.penetrate += 2;
				proj.netUpdate = true;
			}
		}

		protected override void Charging() => AdjustDirection();

		private static readonly BasicEffect effect = new BasicEffect(Main.instance.GraphicsDevice);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//if (firing)
				//return false;
			spriteBatch.End();
			Player player = Main.player[projectile.owner];
			effect.VertexColorEnabled = true;
			Color color = Color.Lerp(new Color(158, 255, 253), lightColor, 0.3f);
			ArrowDraw.DrawArrowBasic(effect, player.Center + direction * 20, direction.ToRotation() + 3.14f, 0 - LerpFloat(0, maxVelocity, charge) * 3, 12,
				color * 0.3f, 0 - LerpFloat(0.7f, 0.4f, charge), 0 - LerpFloat(minVelocity, maxVelocity, charge) * 2);
			ArrowDraw.DrawArrowBasic(effect, player.Center + direction * 20, direction.ToRotation() + 3.14f, 0 - LerpFloat(0, maxVelocity, charge) * 3, 6,
				color * 0.3f, 0 - LerpFloat(0.7f, 0.4f, charge), 0 - LerpFloat(minVelocity, maxVelocity, charge) * 2);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}