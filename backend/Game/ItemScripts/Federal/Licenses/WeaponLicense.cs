using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts.Federal.Licenses
{
    public class WeaponLicense : ItemScript
    {
        public WeaponLicense() : base(379, true)
        {
        }

        public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
        {
            var license = LicenseService.Get(player.LicenseId);
            if(license == null || license.Gun || license.GunRevoked.AddDays(7) > DateTime.Now)
            {
                player.Notify("Information", "Die Lizenz konnte nicht eingelöst werden!", Core.Enums.NotificationType.ERROR);
                return;
            }

            license.Gun = true;
            LicenseService.Update(license);
            InventoryController.RemoveItem(inventory, slot, 1);
        }
    }
}