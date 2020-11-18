using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon.MoonjellySummon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class MoonjellySummonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunazoa Staff");
			Tooltip.SetDefault("Summons a Moonlight Preserver\nMoonlight Preservers summon smaller jellyfish that explode\nOnly one Moonlight Preserver can exist at once\nUsing the staff multiple times takes up summon slots, but increases jellyfish spawn rates");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Summon/MoonjellySummonStaff_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Summon/MoonjellySummonStaff_Glow"),
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

        public override void SetDefaults()
		{
			item.width = 36;
			item.height = 38;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Green;
			item.mana = 10;
			item.damage = 10;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<MoonjellySummon>();
			item.UseSound = SoundID.Item44;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			else
            {
                for (int i = 0; i < 1000; ++i)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                    {
                        Main.projectile[i].minionSlots += 1f;
                        Main.projectile[i].scale += .1f;
                    }
                }
            }
			return base.UseItem(player);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
             player.AddBuff(ModContent.BuffType<MoonjellySummonBuff>(), 3600);
             return player.altFunctionUse != 2;
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    Main.projectile[i].minionSlots += 1f;
					if (Main.projectile[i].scale < 1.3f)
                    Main.projectile[i].scale += .062f;
                }
            }
            return true;
        }
    }
}