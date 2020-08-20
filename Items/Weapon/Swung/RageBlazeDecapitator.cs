using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Swung
{
	public class RageBlazeDecapitator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energized Axe");
			Tooltip.SetDefault("Charged strikes release multiple shards upon damaging enemies");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/RageBlazeDecapitator_Glow");
		}


		public override void SetDefaults()
		{
            item.channel = true;
            item.damage = 36;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.useStyle = 4;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 10;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 2, 10, 0);
            item.rare = 5;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("EnergizedAxeProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Swung/RageBlazeDecapitator_Glow"),
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