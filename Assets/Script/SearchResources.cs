using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchResources : MonoBehaviour {

    public float speed = 60f;
    private bool toFinal = false;
    private Vector3 objective;

    // Use this for initialization
    void Start () {
        this.objective = transform.position;
    }
    
    // Update is called once per frame
    void Update () {
        //if (transform.position.x == objective.x && transform.position.y == objective.y) {
        //    this.changeObjective();
        //}
        
        ////print(transform.position + " move to " + objective);
        //transform.position = Vector3.MoveTowards(transform.position, this.objective, this.speed * Time.deltaTime);
    }

    void changeObjectiveFinal() {
        GameObject player = GameObject.Find("Player's ship");
        
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        pos.x -= 66;
        pos.y += 2;
        this.objective = pos;
        this.toFinal = true;
    }

    void changeObjective() {
        GameObject[] spawns = FindObjectOfType<Spawner>().getCoconuts();//; GetComponent<Spawner>().coconuts;
        var index = 0;
        
        // start to remove
        if (Vector3.Equals(this.objective, transform.position)) {
            for (int i = 0; i < spawns.Length; ++i) {
                if (Vector3.Equals(this.objective, spawns[i].transform.position)) {
                    var foos = new List<GameObject>(spawns);
                    var toRemove = spawns[i];
                    foos.RemoveAt(i);
                    spawns = foos.ToArray();
                    if (toRemove != null) {
                        GameObject.Destroy(toRemove);
                    }
                }
            }
            FindObjectOfType<Spawner>().setCoconuts(spawns);
        }
        // end
        
        index = this.findNearestResource(transform.position, spawns);
        if (index != -1) {
            this.objective = spawns[index].transform.position;
        } else {
            this.changeObjectiveFinal();
        }
    }

    int findNearestResource(Vector3 body, GameObject[] spawns) {
        int index = -1;
        float currentDistance = 0;
        
        for (int i = 0; i < spawns.Length; ++i) {
            if (index == -1) {
                currentDistance = Vector3.Distance(body, spawns[i].transform.position);
                index = i;
            } else {
                float newDist = Vector3.Distance(body, spawns[i].transform.position);
                if (currentDistance > newDist) {
                    index = i;
                    currentDistance = newDist;
                }
            }
        }
        return index;
    }
}
