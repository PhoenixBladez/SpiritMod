using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CryoliteSet
{
	public class CryoStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Staff");
			Tooltip.SetDefault("Shoots out an icy bolt\nOccasionally shoots out a spread of icy bolts\nBoth inflict 'Cryo Crush,' which does more damage as enemy health wanes\nThis effect does not apply to bosses, and deals a flat amount of damage instead\nThese bolts may also slow down enemies");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/CryoliteSet/CryoStaff_Glow");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 9;
			Item.width = 46;
			Item.height = 46;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 0, 70, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CryoliteMage>();
			Item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.rand.NextBool(3))
			{
				Vector2 origVect = velocity;
				for (int X = 0; X < 3; X++)
				{
					Vector2 newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(300, 500) / 10));
					if (Main.rand.NextBool(2))
						newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(300, 500) / 10));

					Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockback, player.whoAmI)];
					proj.friendly = true;
					proj.hostile = false;
					proj.netUpdate = true;
				}
			}
			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/CryoliteSet/CryoStaff_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}