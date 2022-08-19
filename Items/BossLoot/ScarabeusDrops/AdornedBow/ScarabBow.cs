using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.BaseProj;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.ScarabeusDrops.AdornedBow
{
	public class ScarabBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adorned Bow");
			Tooltip.SetDefault("Hold longer for more damage\nConverts wooden arrows into piercing adorned arrows, that get enchanted upon full charge");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}
		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = false;
			Item.shootSpeed = 6.5f;
			Item.crit = 8;
			Item.channel = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<ScarabArrow>();
			}
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<AdornedBowProj>(), damage - Item.damage, knockback, player.whoAmI, type);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);
	}

	public class AdornedBowProj : ChargeBowProj
	{
		public override string Texture => "SpiritMod/Items/BossLoot/ScarabeusDrops/AdornedBow/ScarabBow";

		protected override void SetBowDefaults()
		{
			minDamage = 8;
			maxDamage = 36;
			minVelocity = 10;
			maxVelocity = 18;
			predictor = -1;
			chargeRate = 0.017f;
			dechargeRate = 1f;
		}

		protected override void Shoot(bool firstFire) 
		{
			if (!firstFire)
				return;
			if (Main.myPlayer == Projectile.owner) {
				Projectile proj = Main.projectile[CreateArrow()];
				if (charge >= 1 && Projectile.ai[0] == ModContent.ProjectileType<ScarabArrow>()) proj.penetrate += 2;
				proj.netUpdate = true;
			}
		}

		protected override void Charging() => AdjustDirection();

		public override bool PreDraw(ref Color lightColor)
		{
			//if (firing)
				//return false;
			Main.spriteBatch.End();
			Player player = Main.player[Projectile.owner];
			BasicEffect effect = new BasicEffect(Main.instance.GraphicsDevice) {
				VertexColorEnabled = true
			};
			Color color = Color.Lerp(new Color(158, 255, 253), lightColor, 0.3f);
			ArrowDraw.DrawArrowBasic(effect, player.Center + direction * 20, direction.ToRotation() + 3.14f, 0 - LerpFloat(0, maxVelocity, charge) * 3, 12,
				color * 0.3f, 0 - LerpFloat(0.7f, 0.4f, charge), 0 - LerpFloat(minVelocity, maxVelocity, charge) * 2);
			ArrowDraw.DrawArrowBasic(effect, player.Center + direction * 20, direction.ToRotation() + 3.14f, 0 - LerpFloat(0, maxVelocity, charge) * 3, 6,
				color * 0.3f, 0 - LerpFloat(0.7f, 0.4f, charge), 0 - LerpFloat(minVelocity, maxVelocity, charge) * 2);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}