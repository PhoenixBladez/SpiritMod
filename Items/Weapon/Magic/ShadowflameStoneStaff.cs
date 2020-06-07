using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowflameStoneStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowbreak Wand");
			Tooltip.SetDefault("Holding the item summons erratic shadowflame wisps around the player\nAttacking with the weapon allows these wisps to be controlled by the cursor\nUp to five wisps can exist at once\nInflicts Shadowflame");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 46;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 2;
			item.damage = 12;
			item.knockBack = 4;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 12;
			item.useAnimation = 24;
			item.mana = 10;
			item.magic = true;
            item.channel = true;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("ShadowflameStoneBolt");
			item.shootSpeed = 10f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }
        int counter;
        public override void HoldItem(Player player)
        {
            counter++;
            int spikes = player.GetSpiritPlayer().shadowCount;
            if (counter >= 85 && !player.channel && spikes <= 4)
            {
                player.GetSpiritPlayer().shadowCount++;
                counter = 0;
                int p = Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("ShadowflameStoneBolt"), item.damage, 2f, player.whoAmI, spikes);
                player.GetModPlayer<MyPlayer>().shadowRotation = Main.projectile[p].rotation;
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/ShadowflameStoneStaff_Glow"),
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
