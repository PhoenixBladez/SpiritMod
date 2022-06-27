using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SpiritMod.Items.Weapon.Summon.SacrificialDagger
{
	public class SacrificialDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacrificial Dagger");
			Tooltip.SetDefault("Your summons will target focus enemies\nSummons that hit tagged enemies may deal extra strikes of damage");
		}


		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 44;
			Item.rare = ItemRarityID.Green;
			Item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			Item.damage = 15;
			Item.knockBack = 2;
            Item.mana = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 35;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Summon.SacrificialDagger.SacrificialDaggerProj>();
			Item.shootSpeed = 14;
			Item.UseSound = SoundID.Item1;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.position, 0.46f, .07f, .52f);
            Texture2D texture;
            texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw
            (
                ModContent.Request<Texture2D>("SpiritMod/Items/Weapon/Summon/SacrificialDagger/SacrificialDagger_Glow"),
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
    }
}