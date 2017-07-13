using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AltFight : MonoBehaviour
{
    /*
=======
    public GameObject BegoUnit;

>>>>>>> 2de53f64f1d0e71d93c3fd0ab7fb916f05a468a1
    public Button CaptainRouge;
    public Button CaptainBleu;
    public Button CaptainVert;
    public Button CaptainJaune;
    public Text UnassignedCaptain;

    public Button FastUnitRouge;
    public Button FastUnitBleu;
    public Button FastUnitVert;
    public Button FastUnitJaune;
    public Text UnassignedFastUnit;

    public Button EngineerRouge;
    public Button EngineerBleu;
    public Button EngineerVert;
    public Button EngineerJaune;
    public Text UnassignedEngineer;

    public Button FighterRouge;
    public Button FighterBleu;
    public Button FighterVert;
    public Button FighterJaune;
    public Text UnassignedFighter;

    public Button BegoRouge;
    public Button BegoBleu;
    public Button BegoVert;
    public Button BegoJaune;
    public Text UnassignedBego;

    public Text aiLife;
    public Text playerLife;

    List<CrewMember> crew;

    string selectedRoom = "";

    long playerPreviousTime;
    long playerCooldown = 1000;
    long basePlayerCooldown = 1000;
    int playerDamage = 5;
    long playerRepairValue = 1;
    //long playerRepairCooldown = 5000;
    //long playerRepairPreviousTime;

    long aiPreviousTime;
    long aiCooldown = 2000;
    long baseAICooldown = 2000;
    int aiDamage = 5;
    long aiRepairValue = 1;
    long aiRepairCooldown = 5000;

    long repairCooldown = 5000;
    long repairPreviousTime;

    Player p;
    Player ai;

    // Use this for initialization
    void Start()
    {
        p = PlayerManager.GetInstance().player;
        ai = PlayerManager.GetInstance().ai;

        playerPreviousTime = DateTime.Now.Ticks;
        aiPreviousTime = DateTime.Now.Ticks;
        repairPreviousTime = DateTime.Now.Ticks;

        crew = PlayerManager.GetInstance().player.crew.crewMembers;

        foreach (CrewMember member in crew)
        {
            if (member.type == "Bego")
            {
                GameObject tmp = Instantiate(BegoUnit);
                //tmp.transform.position
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCrew();
        UpdateCooldown();
        if (TimeSpan.FromTicks(DateTime.Now.Ticks - repairPreviousTime).TotalMilliseconds >= repairCooldown)
        {
            Repair();
            repairPreviousTime = DateTime.Now.Ticks;
        }
        if (TimeSpan.FromTicks(DateTime.Now.Ticks - playerPreviousTime).TotalMilliseconds >= playerCooldown)
        {
            PlayerAttack();
            playerPreviousTime = DateTime.Now.Ticks;
        }
        Death();
        if (TimeSpan.FromTicks(DateTime.Now.Ticks - aiPreviousTime).TotalMilliseconds >= aiCooldown)
        {
            AIAttack();
            aiPreviousTime = DateTime.Now.Ticks;
        }
    }

    void UpdateCrew()
    {
        int CaptainRougeCount = 0;
        int CaptainVertCount = 0;
        int CaptainBleuCount = 0;
        int CaptainJauneCount = 0;
        int UnassignedCaptainCount = 0;

        int FastUnitRougeCount = 0;
        int FastUnitVertCount = 0;
        int FastUnitBleuCount = 0;
        int FastUnitJauneCount = 0;
        int UnassignedFastUnitCount = 0;

        int EngineerRougeCount = 0;
        int EngineerVertCount = 0;
        int EngineerBleuCount = 0;
        int EngineerJauneCount = 0;
        int UnassignedEngineerCount = 0;

        int FighterRougeCount = 0;
        int FighterVertCount = 0;
        int FighterBleuCount = 0;
        int FighterJauneCount = 0;
        int UnassignedFighterCount = 0;

        int BegoRougeCount = 0;
        int BegoVertCount = 0;
        int BegoBleuCount = 0;
        int BegoJauneCount = 0;
        int UnassignedBegoCount = 0;

        foreach (CrewMember member in crew)
        {
            if (member.type == "Captain")
            {
                if (member.assignedRoom == "Rouge")
                {
                    CaptainRougeCount += 1;
                }
                else if (member.assignedRoom == "Bleu")
                {
                    CaptainBleuCount += 1;
                }
                else if (member.assignedRoom == "Vert")
                {
                    CaptainVertCount += 1;
                }
                else if (member.assignedRoom == "Jaune")
                {
                    CaptainJauneCount += 1;
                }
                else
                {
                    UnassignedCaptainCount += 1;
                }
            }
            if (member.type == "FastUnit")
            {
                if (member.assignedRoom == "Rouge")
                {
                    FastUnitRougeCount += 1;
                }
                else if (member.assignedRoom == "Bleu")
                {
                    FastUnitBleuCount += 1;
                }
                else if (member.assignedRoom == "Vert")
                {
                    FastUnitVertCount += 1;
                }
                else if (member.assignedRoom == "Jaune")
                {
                    FastUnitJauneCount += 1;
                }
                else
                {
                    UnassignedFastUnitCount += 1;
                }
            }
            if (member.type == "Engineer")
            {
                if (member.assignedRoom == "Rouge")
                {
                    EngineerRougeCount += 1;
                }
                else if (member.assignedRoom == "Bleu")
                {
                    EngineerBleuCount += 1;
                }
                else if (member.assignedRoom == "Vert")
                {
                    EngineerVertCount += 1;
                }
                else if (member.assignedRoom == "Jaune")
                {
                    EngineerJauneCount += 1;
                }
                else
                {
                    UnassignedEngineerCount += 1;
                }
            }
            if (member.type == "Fighter")
            {
                if (member.assignedRoom == "Rouge")
                {
                    FighterRougeCount += 1;
                }
                else if (member.assignedRoom == "Bleu")
                {
                    FighterBleuCount += 1;
                }
                else if (member.assignedRoom == "Vert")
                {
                    FighterVertCount += 1;
                }
                else if (member.assignedRoom == "Jaune")
                {
                    FighterJauneCount += 1;
                }
                else
                {
                    UnassignedFighterCount += 1;
                }
            }
            if (member.type == "Bego")
            {
                if (member.assignedRoom == "Rouge")
                {
                    BegoRougeCount += 1;
                }
                else if (member.assignedRoom == "Bleu")
                {
                    BegoBleuCount += 1;
                }
                else if (member.assignedRoom == "Vert")
                {
                    BegoVertCount += 1;
                }
                else if (member.assignedRoom == "Jaune")
                {
                    BegoJauneCount += 1;
                }
                else
                {
                    UnassignedBegoCount += 1;
                }
            }
        }

        CaptainRouge.GetComponentInChildren<Text>().text = CaptainRougeCount.ToString();
        CaptainBleu.GetComponentInChildren<Text>().text = CaptainBleuCount.ToString();
        CaptainVert.GetComponentInChildren<Text>().text = CaptainVertCount.ToString();
        CaptainJaune.GetComponentInChildren<Text>().text = CaptainJauneCount.ToString();
        UnassignedCaptain.text = UnassignedCaptainCount.ToString();

        FastUnitRouge.GetComponentInChildren<Text>().text = FastUnitRougeCount.ToString();
        FastUnitBleu.GetComponentInChildren<Text>().text = FastUnitBleuCount.ToString();
        FastUnitVert.GetComponentInChildren<Text>().text = FastUnitVertCount.ToString();
        FastUnitJaune.GetComponentInChildren<Text>().text = FastUnitJauneCount.ToString();
        UnassignedFastUnit.text = UnassignedFastUnitCount.ToString();

        EngineerRouge.GetComponentInChildren<Text>().text = EngineerRougeCount.ToString();
        EngineerBleu.GetComponentInChildren<Text>().text = EngineerBleuCount.ToString();
        EngineerVert.GetComponentInChildren<Text>().text = EngineerVertCount.ToString();
        EngineerJaune.GetComponentInChildren<Text>().text = EngineerJauneCount.ToString();
        UnassignedEngineer.text = UnassignedEngineerCount.ToString();

        FighterRouge.GetComponentInChildren<Text>().text = FighterRougeCount.ToString();
        FighterBleu.GetComponentInChildren<Text>().text = FighterBleuCount.ToString();
        FighterVert.GetComponentInChildren<Text>().text = FighterVertCount.ToString();
        FighterJaune.GetComponentInChildren<Text>().text = FighterJauneCount.ToString();
        UnassignedFighter.text = UnassignedFighterCount.ToString();

        BegoRouge.GetComponentInChildren<Text>().text = BegoRougeCount.ToString();
        BegoBleu.GetComponentInChildren<Text>().text = BegoBleuCount.ToString();
        BegoVert.GetComponentInChildren<Text>().text = BegoVertCount.ToString();
        BegoJaune.GetComponentInChildren<Text>().text = BegoJauneCount.ToString();
        UnassignedBego.text = UnassignedBegoCount.ToString();
    }

    void UpdateCooldown()
    {
        playerCooldown = basePlayerCooldown;
        foreach (CrewMember member in crew)
        {
            if (member.assignedRoom == "Rouge")
            {
                playerCooldown -= member.canonReloadSpeed;
            }
        }
    }

    void Repair()
    {
        p.life += (int)playerRepairValue;
        ai.life += (int)aiRepairValue;
        foreach (CrewMember member in crew)
        {
            if (member.assignedRoom == "Vert")
            {
                p.life += (int)member.repairSpeed;
            }
        }
    }

    void PlayerAttack()
    {
        ai.life -= playerDamage;
        aiLife.text = ai.life.ToString();
    }

    void AIAttack()
    {
        p.life -= aiDamage;
        playerLife.text = p.life.ToString();
    }

    void Death()
    {
        if (p.life == 0)
        {
            print("PERDU");
        }
        else if (ai.life == 0)
        {
            print("VICTORE");
        }
    }

    public void SelectRouge()
    {
        selectedRoom = "Rouge";
    }

    public void SelectVert()
    {
        selectedRoom = "Vert";
    }

    public void SelectBleu()
    {
        selectedRoom = "Bleu";
    }

    public void SelectJaune()
    {
        selectedRoom = "Jaune";
    }

    public void AssignCaptain()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Captain" && member.assignedRoom == "")
            {
                member.assignedRoom = selectedRoom;
            }
        }
    }

    public void AssignFastUnit()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "FastUnit" && member.assignedRoom == "")
            {
                member.assignedRoom = selectedRoom;
            }
        }
    }

    public void AssignEngineer()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Engineer" && member.assignedRoom == "")
            {
                member.assignedRoom = selectedRoom;
            }
        }
    }

    public void AssignFighter()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Fighter" && member.assignedRoom == "")
            {
                member.assignedRoom = selectedRoom;
            }
        }
    }

    public void AssignBego()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Bego" && member.assignedRoom == "")
            {
                member.assignedRoom = selectedRoom;
            }
        }
    }

    public void UnassignCaptain()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Captain" && member.assignedRoom == selectedRoom)
            {
                member.assignedRoom = "";
            }
        }
    }

    public void UnassignFastUnit()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "FastUnit" && member.assignedRoom == selectedRoom)
            {
                member.assignedRoom = "";
            }
        }
    }

    public void UnassignEngineer()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Engineer" && member.assignedRoom == selectedRoom)
            {
                member.assignedRoom = "";
            }
        }
    }

    public void UnassignFighter()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Fighter" && member.assignedRoom == selectedRoom)
            {
                member.assignedRoom = "";
            }
        }
    }

    public void UnassignBego()
    {
        foreach (CrewMember member in crew)
        {
            if (member.type == "Bego" && member.assignedRoom == selectedRoom)
            {
                member.assignedRoom = "";
            }
        }
    }*/
}
