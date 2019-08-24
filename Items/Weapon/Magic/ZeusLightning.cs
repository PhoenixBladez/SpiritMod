using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Weapon.Magic
{
    public class ZeusLightning : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zeus's Lightning");
			Tooltip.SetDefault("'Wield the power of Zeus'");
		}


        public override void SetDefaults()
        {
            item.damage = 44; //The damage
            item.magic = true; //Whether or not it is a magic weapon
            item.width = 54; //Item width
            item.height = 54; //Item height
            item.maxStack = 1; //How many of this item you can stack
            item.useTime = 60; //How long it takes for the item to be used
            item.useAnimation = 60; //How long the animation of the item takes
            Item.staff[item.type] = true;
            item.knockBack = 7f; //How much knockback the item produces
            item.noMelee = true; //Whether the weapon should do melee damage or not
            item.useStyle = 5; //How the weapon is held, 5 is the gun hold style
            item.value = 1; //How much the item is worth
            item.rare = 4; //The rarity of the item
            item.shoot = 580; //What the item shoots, retains an int value | *
            item.shootSpeed = 7f; //How fast the projectile fires   
            item.mana = 20;
            item.autoReuse = true; //Whether it automatically uses the item again after its done being used/animated
        }

		   public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
           Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Thunder"));
            Vector2 vector82 = -Main.player[Main.myPlayer].Center + Main.MouseWorld;
            float ai = Main.rand.Next(100);
            Vector2 vector83 = Vector2.Normalize(vector82) * item.shootSpeed;
            Projectile.NewProjectile(player.Center.X, player.Center.Y, vector83.X, vector83.Y, type, damage, .5f, player.whoAmI, vector82.ToRotation(), ai);
            return false;
        }
    }
}