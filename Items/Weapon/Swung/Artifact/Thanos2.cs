using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Artifact;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
    public class Thanos2 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shard of Thanos");
            Tooltip.SetDefault("Shoots out an afterimage of the Shard\nRight-click to summon two storms of rotating crystals around the player\nMelee or afterimage attacks may crystallize enemies, stopping them in place");

        }


        public override void SetDefaults() {
            item.damage = 41;
            item.melee = true;
            item.width = 44;
            item.height = 42;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 50);
            item.shoot = mod.ProjectileType("Thanos2Proj");
            item.rare = 4;
            item.shootSpeed = 9f;
            item.UseSound = SoundID.Item69;
            item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            if(player.GetSpiritPlayer().Resolve) {
                player.AddBuff(ModContent.BuffType<Resolve>(), 2);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Crystal>());
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if(Main.rand.Next(8) == 1) {
                target.AddBuff(ModContent.BuffType<Crystallize>(), 180);
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {
                float kb = knockBack * .2f;
                int shield = (int)(damage * .375f);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 45, player.Center.Y + 45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y + 40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y - 40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, kb, player.whoAmI);

                shield = (int)(damage * .65625);
                Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, kb, player.whoAmI);
                return false;
            }
            return true;
        }
    }
}