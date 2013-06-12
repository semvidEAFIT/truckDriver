package configurationtool.controller;

import configurationtool.model.Configuration;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.concurrent.CancellationException;
import org.json.JSONException;

/**
 *
 * @author halzate93
 */
public class Controller {

    private Configuration configuration;
    private DAO dao;

    public Controller() {
        dao = new DAO();
        configuration = new Configuration();
    }

    public void saveConfiguration() throws IOException, NullPointerException, CancellationException {
        dao.saveConfiguration(configuration);
    }

    public Configuration loadConfiguration() throws FileNotFoundException, NullPointerException, JSONException, CancellationException {
        configuration = dao.loadConfiguration();
        return configuration;
    }

    public Configuration getConfiguration() {
        return configuration;
    }
}
