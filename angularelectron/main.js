/**
 * Include our app
 */
const {app, BrowserWindow } = require('electron');
const path = require('path');
// browser-window creates a native window
let mainWindow = null;

const os = require('os');
var apiProcess = null;

function startApi() {
  var proc = require('child_process').spawn;
  //  run server
  var apipath = path.join(__dirname, '\\api\\bin\\dist\\win\\corepacs.exe');
  console.log(apipath);
  if (os.platform() === 'darwin') {
    apipath = path.join(__dirname, '//api//bin//dist//osx//corepacs')
  }
  console.log('Calling the processes');
  apiProcess = proc(apipath);
  console.log('finished calling the exe');

  apiProcess.stdout.on('data', (data) => {
    writeLog(`stdout: ${data}`);
    if (mainWindow == null) {
      createWindow();
    }
  });
}

//Kill process when electron exits
process.on('exit', function () {
  writeLog('exit');
  apiProcess.kill();
});

function writeLog(msg){
  console.log(msg);
}

app.on('window-all-closed', () => {
  // On macOS it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

const createWindow = () => {
  // Initialize the window to our specified dimensions
  mainWindow = new BrowserWindow({ width: 1200, height: 900 });

  // Tell Electron where to load the entry point from
  mainWindow.loadURL('file://' + __dirname + '/src/app/index.html');
  
  console.log("window Launched");
  // Open the DevTools.
  mainWindow.webContents.openDevTools();
  console.log("launching the api");
  startApi();
  console.log("launching the api");
  // Clear out the main window when the app is closed
  mainWindow.on('closed', () => {
    mainWindow = null;
  });
};

app.on('ready', createWindow);

app.on('activate', () => {
  // On macOS it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (mainWindow === null) {
    createWindow();
  }
});

