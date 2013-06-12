package configurationtool.model;

import java.awt.Point;
import org.json.JSONException;
import org.json.JSONObject;
import org.omg.CORBA.DynAnyPackage.InvalidValue;

/**
 *
 * @author halzate93
 */
public class Difficulty {

    private int nodeCount;
    private int time;
    private float errorMargin;
    private Point dimensions;
    private DifficultyEnum level;

    public Difficulty(int nodeCount, int time, float errorMargin, Point dimensions, DifficultyEnum level) {
        this.nodeCount = nodeCount;
        this.time = time;
        this.errorMargin = errorMargin;
        this.dimensions = dimensions;
        this.level = level;
    }

    public Difficulty(DifficultyEnum level, JSONObject jsonObject) throws JSONException {
        this.level = level;
        this.nodeCount = jsonObject.getInt("nodeCount");
        this.time = jsonObject.getInt("time");
        this.errorMargin = (float) jsonObject.getDouble("errorMargin");
        JSONObject dimensionsJO = jsonObject.getJSONObject("dimensions");
        dimensions = new Point(dimensionsJO.getInt("x"), dimensionsJO.getInt("y"));
    }

    public Difficulty(DifficultyEnum level) {
        this.level = level;
        time = 20;
        nodeCount = 5;
        dimensions = new Point(5, 5);
    }

    public JSONObject getJsonObject() {
        JSONObject jsonObject = new JSONObject();
        jsonObject.put("time", getTime());
        jsonObject.put("nodeCount", getNodeCount());
        jsonObject.put("errorMargin", getErrorMargin());
        JSONObject dimensionsJO = new JSONObject();
        dimensionsJO.put("x", (int) getDimensions().getX());
        dimensionsJO.put("y", (int) getDimensions().getY());
        jsonObject.put("dimensions", dimensionsJO);
        return jsonObject;
    }

    public int getNodeCount() {
        return nodeCount;
    }

    public int getTime() {
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

    public void setNodeCount(int nodeCount) throws InvalidValue {
        if (nodeCount < 5) {
            throw new InvalidValue("El número de nodos no puede ser tan pequeño.");
        }
        this.nodeCount = nodeCount;
    }

    public void setTime(int time) throws InvalidValue{
        if(time < 20){
            throw new InvalidValue("El tiempo no puede ser tan corto.");
        }
        this.time = time;
    }

    public void setErrorMargin(float errorMargin) throws InvalidValue {
        if(errorMargin < 0.0){
            throw new InvalidValue("El margen de error no puede ser negativo");
        }
        this.errorMargin = errorMargin;
    }

    public void setDimensions(Point dimensions) throws InvalidValue {
        if(dimensions.getX() < 5 || dimensions.getY() < 5){
            throw new InvalidValue("Las dimensiones no son permitidas.");
        }
        this.dimensions = dimensions;
    }

    public void setDimensionsX(int x) throws InvalidValue {
        if(x < 5){
             throw new InvalidValue("Las dimensiones no son permitidas.");
        }
        dimensions.x = x;
    }

    public void setDimensionsY(int y) throws InvalidValue {
        if(y < 5){
             throw new InvalidValue("Las dimensiones no son permitidas.");
        }
        dimensions.x = y;
    }
}
