package configurationtool.model;

import java.awt.Point;
import org.json.JSONObject;
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

/**
 *
 * @author halzate93
 */
public class Difficulty {
    private int nodeCount;
    private float time;
    private float errorMargin;
    private Point dimensions;
    private DifficultyEnum level;

    public Difficulty(int nodeCount, float time, float errorMargin, Point dimensions, DifficultyEnum level) {
        this.nodeCount = nodeCount;
        this.time = time;
        this.errorMargin = errorMargin;
        this.dimensions = dimensions;
        this.level = level;
    }
    
    public Difficulty(JSONObject jsonObject){
        throw new NotImplementedException();
        //TODO --- initialize object from json parameters
    }

    public Difficulty(DifficultyEnum level) {
        this.level = level;
        dimensions = new Point();
    }
    
    public JSONObject getJsonObject(){
        throw new NotImplementedException();
        //TODO --- serialize object parameters in json form
    }

    public int getNodeCount() {
        return nodeCount;
    }

    public float getTime() {
        return time;
    }

    public float getErrorMargin() {
        return errorMargin;
    }

    public Point getDimensions() {
        return dimensions;
    }

    public DifficultyEnum getLevel() {
        return level;
    }
}
