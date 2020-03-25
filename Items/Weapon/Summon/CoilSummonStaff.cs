using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class CoilSummonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Rod");
			Tooltip.SetDefault("Summons a stationary electric turret\nThis turret shoots beams of lightning that jump from enemy to enemy");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/CoilSummonStaff_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Summon/CoilSummonStaff_Glow"),
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
        }////

        public override void SetDefaults()
		{
		item.CloneDefaults(ItemID.QueenSpiderStaff);
			item.damage = 16; 
			item.mana = 12;   
			item.width = 40;
			item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
            item.rare = 2;
            item.knockBack = 2.5f;
			item.UseSound = SoundID.Item25;
			item.shoot = mod.ProjectileType("CoilSentrySummon");
			item.shootSpeed = 0f;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.type == item.shoot && p.owner == player.whoAmI) 
				{
					p.active = false;
				}
			}
            Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "TechDrive", 6);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
