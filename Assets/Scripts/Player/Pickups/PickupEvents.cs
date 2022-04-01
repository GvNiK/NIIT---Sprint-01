using System;
using UnityEngine;

public class PickupEvents 
{
    public Action<Pickup> OnPickupEventCollected = delegate { };
}
