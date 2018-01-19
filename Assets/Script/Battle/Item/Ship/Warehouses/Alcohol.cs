using System;
using System.Collections.Generic;

public class Alcohol : Warehouse
{
    public Alcohol() : base(40, Ship_Item.ALCOHOL)
    { }

    protected override void applyMalusOnNotWorking()
    {
        this.getParentShip().applyCrewAttributes(Effect.MORAL, 0, 50);
    }

    protected override void applyChangeOnRevive()
    {
        this.getParentShip().removeCrewAttributes(Effect.MORAL);
    }
}
