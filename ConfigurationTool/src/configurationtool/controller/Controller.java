package configurationtool.controller;

import configurationtool.model.Configuration;

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
    
    public void saveConfiguration(){
        dao.saveConfiguration(configuration);
    }
    
    public Configuration loadConfiguration(){
        configuration = dao.loadConfiguration();
        return configuration;
    }

    public Configuration getConfiguration() {
        return configuration;
    }
}
