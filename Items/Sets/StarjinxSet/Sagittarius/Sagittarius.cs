using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class Sagittarius : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sagittarius");
			Tooltip.SetDefault("Creates a constellation behind you as you shoot\nConstellation stars will fire additional astral arrows towards the cursor");
			//SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 78;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 3;
			Item.channel = true;
			//item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast");
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 2);
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.shootSpeed = 22f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow"), rotation, scale);

		public override void HoldItem(Player player)
		{
			if (player == Main.LocalPlayer)
			{
				int fireTime = 3 * (Item.useAnimation / 9);

				if (!player.channel && player.itemAnimation > fireTime)
				{
					player.itemTime = 0;
					player.itemAnimation = 0;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.ItemAnimation, -1, -1, null, player.whoAmI);
					return;
				}

				if (player.itemAnimation > 0)
				{
					player.ChangeDir(player.DirectionTo(Main.MouseWorld).X > 0 ? 1 : -1);
					player.itemRotation = MathHelper.WrapAngle(player.AngleTo(Main.MouseWorld) - ((player.direction < 0) ? MathHelper.Pi : 0)) - player.fullRotation;
					if (Main.netMode != NetmodeID.SinglePlayer)
						NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, player.whoAmI);
				}

				if (player.itemAnimation == fireTime)
				{
					int type = ModContent.ProjectileType<SagittariusArrow>();
					Vector2 shootDir = Vector2.UnitX.RotatedBy(player.AngleTo(Main.MouseWorld));
					Projectile constellation = Projectile.NewProjectileDirect(Item.GetSource_ItemUse(Item), player.MountedCenter - shootDir.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(70, 100), Vector2.Zero, ModContent.ProjectileType<SagittariusConstellation>(), (int)(Item.damage * 0.75f), Item.knockBack, player.whoAmI, 4, -1);

					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/StarCast"), player.Center);
					Projectile arrow = Projectile.NewProjectileDirect(Item.GetSource_ItemUse(Item), player.MountedCenter + (shootDir * 20), shootDir * Item.shootSpeed, type, Item.damage, Item.knockBack, player.whoAmI);

					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, constellation.whoAmI);
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, arrow.whoAmI);
					}
				}
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)  => false;

		public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}