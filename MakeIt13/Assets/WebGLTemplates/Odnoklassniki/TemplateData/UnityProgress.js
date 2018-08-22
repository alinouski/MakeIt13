/*function UnityProgress (gameInstance, progress)
{
  this.progress = 0.0;
  this.message = "";
      
  this.SetProgress = function (progress) { 
    if (this.progress < progress)
      this.progress = progress; 
    if (this.progress == 1) {
      document.getElementById("loadingBox").style.display = "none";
      document.getElementById("bgBar").style.display = "none";
      document.getElementById("progressBar").style.display = "none";
    } 
    else if(this.progress >=0.9){
      this.SetMessage("New test 1 " + this.progress);
    }
    else if(this.progress >=0.5){
      this.SetMessage("New test 2 " + this.progress);
    }
    else if(this.progress >=0.2){
      this.SetMessage("New test 3 " + this.progress);
    }
    this.Update();
  }

  this.SetMessage = function (message) { 
    this.message = message; 
    this.Update();
  }

  this.Clear = function() {
    document.getElementById("loadingBox").style.display = "none";
  }
  
  this.Update = function() {
    var length = 200 * Math.min(this.progress, 1);
    bar = document.getElementById("progressBar");
    bar.style.width = length + "px";
    document.getElementById("loadingInfo").innerHTML = this.message;
  }

  this.SetProgress(progress);
}
*/
function UnityProgress(gameInstance, progress) {
  if (!gameInstance.Module)
    return;

  if (!gameInstance.loadingBox) {
    gameInstance.loadingBox = document.createElement('div');
    gameInstance.loadingBox.id = "loadingBox";
    gameInstance.container.appendChild(gameInstance.loadingBox);
  }
  if (!gameInstance.bgBar) {
    gameInstance.bgBar = document.createElement("div");
    gameInstance.bgBar.id = "bgBar";
    gameInstance.loadingBox.appendChild(gameInstance.bgBar);
  }

  if (!gameInstance.progressBar) {
    gameInstance.progressBar = document.createElement("div");
    gameInstance.progressBar.id = "progressBar";
    gameInstance.loadingBox.appendChild(gameInstance.progressBar);
  }

  if (!gameInstance.loadingInfo) {
    gameInstance.loadingInfo = document.createElement("p");
    gameInstance.loadingInfo.id = "loadingInfo";
    gameInstance.loadingInfo.textContent = "Downloading...";
    gameInstance.loadingBox.appendChild(gameInstance.loadingInfo);
  }

  var length = 200 * Math.min(progress, 1);
  gameInstance.progressBar.style.width = length + "px";
  gameInstance.loadingInfo.textContent = "Downloading... " + Math.round(progress * 100) + "%";
  if (progress == 1) {
    gameInstance.loadingBox.style.display = "none";
  }
}