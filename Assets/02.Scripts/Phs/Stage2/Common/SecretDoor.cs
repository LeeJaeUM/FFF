using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : DoorBase
{
    private void Start()
    {
        BookShelf_Unlock unlock = FindAnyObjectByType<BookShelf_Unlock>();
        if (unlock != null)
        {
            unlock.onSecretRoom = DoorOpen;
        }
    }
}
