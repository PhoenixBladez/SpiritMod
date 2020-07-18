using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CryoStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Staff");
			Tooltip.SetDefault("Shoots out an icy bolt\nOccasionally shoots out a spread of icy bolts\nBoth inflict 'Cryo Crush,' which does more damage as enemy health wanes\nThis effect does not apply to bosses, and deals a flat amount of damage instead\nThese bolts may also slow down enemies");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/CryoStaff_Glow");
		}

		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.damage = 32;
			item.magic = true;
			item.mana = 9;
			item.width = 46;
			item.height = 46;
			item.useTime = 27;
			item.useAnimation = 27;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 4.5f;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CryoliteMage>();
			item.shootSpeed = 8f;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/CryoStaff_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
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
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(Main.rand.Next(3) == 0) {
				Vector2 origVect = new Vector2(speedX, speedY);
				for(int X = 0; X < 3; X++) {
					if(Main.rand.Next(2) == 1) {
						newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(300, 500) / 10));
					} else {
						newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(300, 500) / 10));
					}
					Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI)];
					proj.friendly = true;
					proj.hostile = false;
					proj.netUpdate = true;
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
