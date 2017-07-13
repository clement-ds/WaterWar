using System;
using System.Collections.Generic;

public class Alcohol : Warehouse
{
    public Alcohol() : base(40)
    { }

    protected override void applyMalusOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().applyCrewAttributes(Effect.MORAL, 0, 50);
    }
}
