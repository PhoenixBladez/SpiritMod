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
			Tooltip.SetDefault("Your summons will target struck enemies\nSummons that hit tagged enemies may deal extra strikes of damage");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 44;
			item.rare = ItemRarityID.Green;
			item.value = Terraria.Item.sellPrice(0, 1, 70, 0);
			item.damage = 15;
			item.knockBack = 2;
            item.mana = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 35;
			item.summon = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Summon.SacrificialDagger.SacrificialDaggerProj>();
			item.shootSpeed = 14;
			item.UseSound = SoundID.Item1;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.46f, .07f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Summon/SacrificialDagger/SacrificialDagger_Glow"),
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
    }
}