using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class Moonshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonshot");
			Tooltip.SetDefault("Hold to charge up an electric wave\n'Aim for the stars!'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Gun/Moonshot_Glow");
        }

		public override void SetDefaults()
		{
			item.channel = true;
			item.damage = 12;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 25;
            item.reuseDelay = 10;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.useAmmo = AmmoID.Bullet;
			item.value = Terraria.Item.sellPrice(0, 1, 42, 0);
			item.rare = 2;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MoonshotProj>();
			item.shootSpeed = 10f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Gun/Moonshot_Glow"),
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
			type = ModContent.ProjectileType<MoonshotProj>();
			return true;
		}

		public override bool ConsumeAmmo(Player player) => (player.ownedProjectileCounts[ModContent.ProjectileType<MoonshotProj>()] > 0); //dont consume ammo on initial use, but let it consume ammo for player.pickammo
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}