function UnityProgress(gameInstance, progress) {
  if (!gameInstance.Module)
    return;
  if (!gameInstance.progress) {    
    gameInstance.progress = document.createElement("div");
    gameInstance.progress.className = "progress " + gameInstance.Module.splashScreenStyle;
    gameInstance.progress.empty = document.createElement("div");
    gameInstance.progress.empty.className = "empty";
    gameInstance.progress.appendChild(gameInstance.progress.empty);
    gameInstance.progress.full = document.createElement("div");
    gameInstance.progress.full.className = "full";
    gameInstance.progress.appendChild(gameInstance.progress.full);
	
	gameInstance.progress.cover = document.createElement("div");
    gameInstance.progress.cover.className = "cover";
    gameInstance.progress.appendChild(gameInstance.progress.cover);
	
    gameInstance.container.appendChild(gameInstance.progress);
  }
  //创建一个html标签
  if(!ele)
  {
	ele = document.createElement("div");
    ele.className = "title";
    gameInstance.progress.appendChild(ele);	
  }
  if(!tip)
  {
	tip = document.createElement("div");
    tip.className = "tip";
    gameInstance.progress.appendChild(tip);	
	tip.innerHTML = getComputedStyle(document.documentElement).getPropertyValue('--tipContent');
  }
  ele.innerHTML = (parseInt(100 * progress)) + "%";
  gameInstance.progress.full.style.width = (100 * progress) + "%";
  if(progress < 0.09)
  {
	positionCover = "720px";
  }
  else
  {
	positionCover = (1 - progress) * 800 - 10 + "px";
  }
  gameInstance.progress.cover.style.right = positionCover;
  //gameInstance.progress.empty.style.width = (100 * (1 - progress)) + "%";
  if (progress == 1)
  {
	gameInstance.progress.style.display = "none";
	document.getElementById("version").hidden = true;
	document.getElementById("explanation").hidden = true;
  }
}
 var ele ; 
 var tip ;
 var cover;
 var positionCover;