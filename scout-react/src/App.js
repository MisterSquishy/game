import React, { Component } from 'react';
import { Unity } from 'react-unity-webgl';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        <Unity src="Build/scout.json" />
      </div>
    );
  }
}

export default App;
