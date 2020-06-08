using Terraria.ID;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class TikiJavelin : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tiki Javelin");
            Tooltip.SetDefault("Hold and release to throw\nHold it longer for more velocity and damage");
            //  SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Equipment/StarMap_Glow");
        }

        public override void SetDefaults() {
            item.damage = 20;
            item.noMelee = true;
            item.channel = true; //Channel so that you can held the weapon [Important]
            item.rare = 3;
            item.width = 18;
            item.height = 18;
            item.useTime = 15;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 24;
            item.melee = true;
            item.noMelee = true;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<TikiJavelinProj>();
            item.shootSpeed = 0f;
        }
        /*   public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
           {
               Lighting.AddLight(item.position, 0.08f, .28f, .38f);
               Texture2D texture;
               texture = Main.itemTexture[item.type];
               spriteBatch.Draw
               (
                   ModContent.GetTexture("SpiritMod/Items/Equipment/StarMap_Glow"),
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
           }*/
    }
}
