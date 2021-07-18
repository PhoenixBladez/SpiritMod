using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class Sagittarius : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sagittarius");
			Tooltip.SetDefault("Creates a constellation behind you as you shoot\nConstellation stars will fire additional astral arrows towards the cursor");
			//SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.noMelee = true;
			item.ranged = true;
			item.width = 40;
			item.height = 78;
			item.useTime = 70;
			item.useAnimation = 70;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.channel = true;
			//item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast");
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(gold: 2);
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.shootSpeed = 22f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override void HoldItem(Player player)
		{
			if(player == Main.LocalPlayer)
			{
				int fireTime = 3 * (item.useAnimation / 9);
				if (!player.channel && player.itemAnimation > fireTime)
				{
					player.itemTime = 0;
					player.itemAnimation = 0;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.ItemAnimation, -1, -1, null, player.whoAmI);
					return;
				}
				if(player.itemAnimation > 0)
				{
					player.ChangeDir(player.DirectionTo(Main.MouseWorld).X > 0 ? 1 : -1);
					player.itemRotation = MathHelper.WrapAngle(player.AngleTo(Main.MouseWorld) - ((player.direction < 0) ? MathHelper.Pi : 0)) - player.fullRotation;
					if(Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, player.whoAmI);
				}

				if (player.itemAnimation == fireTime)
				{
					int type = ModContent.ProjectileType<SagittariusArrow>();
					Vector2 shootDir = Vector2.UnitX.RotatedBy(player.AngleTo(Main.MouseWorld));
					Projectile constellation = Projectile.NewProjectileDirect(player.MountedCenter - shootDir.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(70, 150),
						Vector2.Zero, ModContent.ProjectileType<SagittariusConstellation>(), (int)(item.damage * 0.75f), item.knockBack, player.whoAmI, 4, -1);

					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast"), player.Center);
					Projectile arrow = Projectile.NewProjectileDirect(player.MountedCenter + (shootDir * 20), shootDir * item.shootSpeed, type, item.damage, item.knockBack, player.whoAmI);

					if(Main.netMode != NetmodeID.SinglePlayer)
					{
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, constellation.whoAmI);
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, arrow.whoAmI);
					}
				}
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) => false;

		public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "Starjinx", 14);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}